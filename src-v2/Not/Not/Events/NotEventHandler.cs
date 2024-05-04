namespace Not.Events;

internal delegate void NotEventHandler();

internal delegate void NotHandler<T>(T payload);
