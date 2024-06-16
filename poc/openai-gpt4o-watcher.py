import imageio
from openai import OpenAI
import base64


def encode_frame(frame):
    frame_bytes = imageio.imwrite(imageio.RETURN_BYTES, frame, 'jpg')
    frame_base64 = base64.b64encode(frame_bytes).decode('utf-8')
    return frame_base64


def group_frames_by_second(video_path):
    reader = imageio.get_reader(video_path, 'ffmpeg')
    fps = reader.get_meta_data()['fps']
    if fps == 0:
        fps = 30  # Default to 30 if unable to get FPS

    frames_to_skip = int(fps / 10)
    frames_by_second = {}
    frame_count = 0

    for i, frame in enumerate(reader):
        if i % frames_to_skip != 0:
            continue

        second = int(i // fps)
        if second not in frames_by_second:
            frames_by_second[second] = []

        frames_by_second[second].append(frame)
        frame_count += 1

    reader.close()
    print(f'grouped {frame_count} frames')
    return frames_by_second


def encode_frames(frames_by_second):
    encoded_frames_by_second = {}
    for s, frames in frames_by_second.items():
        encoded_frames_by_second[s] = []
        for frame in frames:
            envoded = encode_frame(frame)
            encoded_frames_by_second[s].append(envoded)

    return encoded_frames_by_second


frames_per_second = group_frames_by_second("../video-one.mp4")
encoded_frames = encode_frames(frames_per_second)

print(f'starting OpenAI connection')
client = OpenAI(api_key="")
MODEL = "gpt-4o"

content = (
    "You are a system that detects when a Combination (a human rider and it's horse) cross a finish line. "
    "You will be given a sequence of images (frames spanning 1 second period) and you have to respond in one of 2 ways:"
    " 1) if no Combinations cross you should respond 'NO'"
    " 2) if a Combination crosses then you should respond 'YES: <combination-number>'"
    " where <combination-number> is a placeholder for the Number of the Combination."
    "The finish line is represented by an arch"
    " consisting of 2 vertical stands and a horizontal beam that connects them."
    "A Combination is considered having crossed the finish line the moment"
    " the horse's head 'touches' the imaginary vertical plane of the finish line."
    "The Number of the Combination can be seen as a white number on the chest of a vest the human rider is carrying."
    "You should ignore Combinations that have already crossed the finish line. ")

for i in encoded_frames:
    frames = encoded_frames[i]
    print(f'sending request {i} to {MODEL}')
    response = client.chat.completions.create(
        model=MODEL,
        messages=[
            {"role": "system", "content": content},
            {"role": "user", "content": [
                "These are the video frames representing 1 second period.",
                *map(lambda x: {
                    "type": "image_url",
                    "image_url": {"url": f'data:image/jpg;base64,{x}', "detail": "low"}
                }, frames)
            ]}
        ]
    )
    print(response.choices[0].message.content)