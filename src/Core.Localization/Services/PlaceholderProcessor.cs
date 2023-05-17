using Core.ConventionalServices;
using System.Collections.Generic;
using static Core.Localization.LocalizationConstants.Placeholders;

namespace Core.Localization.Services;

public class PlaceholderProcessor : IPlaceholderProcessor
{
    public Dictionary<string, string> Process(Dictionary<string, string> values)
    {
        var processed = new Dictionary<string, string>();
        foreach (var (key, template) in values)
        {
            var replaced = this.Replace(template, values);
            processed.Add(key, replaced);
        }

        return processed;
    }

    public string Replace(string template, Dictionary<string, string> values)
    {
        template = template.Replace(ATHLETE,                  values[nameof(Strings.ATHLETE_ENTITY)]);
        template = template.Replace(ATHLETES,                 values[nameof(Strings.ATHLETES_ENTITY)]);
        template = template.Replace(COMPETITION,              values[nameof(Strings.COMPETITION_ENTITY)]);
        template = template.Replace(COMPETITIONS,             values[nameof(Strings.COMPETITIONS_ENTITY)]);
        template = template.Replace(COUNTRY,                  values[nameof(Strings.COUNTRY_ENTITY)]);
        template = template.Replace(COUNTRIES,                values[nameof(Strings.COUNTRIES_ENTITY)]);
        template = template.Replace(EVENT,                    values[nameof(Strings.ENDURANCE_EVENT_ENTITY)]);
        template = template.Replace(EVENTS,                   values[nameof(Strings.ENDURANCE_EVENTS_ENTITY)]);
        template = template.Replace(HORSE,                    values[nameof(Strings.HORSE_ENTITY)]);
        template = template.Replace(HORSES,                   values[nameof(Strings.HORSES_ENTITY)]);
        template = template.Replace(PARTICIPANT,              values[nameof(Strings.PARTICIPANT_ENTITY)]);
        template = template.Replace(PARTICIPANTS,             values[nameof(Strings.PARTICIPANTS_ENTITY)]);
        template = template.Replace(PARTICIPATION,            values[nameof(Strings.PARTICIPATION_ENTITY)]);
        template = template.Replace(PARTICIPATIONS,           values[nameof(Strings.PARTICIPATIONS_ENTITY)]);
        template = template.Replace(PERFORMANCE,              values[nameof(Strings.PERFORMANCE_ENTITY)]);
        template = template.Replace(PERFORMANCES,             values[nameof(Strings.PERFORMANCES_ENTITY)]);
        template = template.Replace(PERSONNEL,                values[nameof(Strings.PERSONNEL_ENTITY)]);
        template = template.Replace(RESULT,                   values[nameof(Strings.RESULT_ENTITY)]);
        template = template.Replace(RESULTS,                  values[nameof(Strings.RESULTS_ENTITY)]);
        template = template.Replace(LAP,                      values[nameof(Strings.LAP_ENTITY)]);
        template = template.Replace(LAPS,                     values[nameof(Strings.LAPS_ENTITY)]);
        template = template.Replace(WORDS_UPDATE,             values[nameof(Strings.UPDATE)]);
        template = template.Replace(WORDS_SELECT_DIRECTORY,   values[nameof(Strings.SELECT_DIRECTORY)]);
        template = template.Replace(WORDS_ORDER,              values[nameof(Strings.ORDER)]);

        return template;
    }
}

public interface IPlaceholderProcessor : ITransientService
{
    Dictionary<string, string> Process(Dictionary<string, string> values);
}
