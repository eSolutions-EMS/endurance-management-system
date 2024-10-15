namespace Not.Events;

public delegate void NotEventHandler();

public delegate void NotHandler<T>(T payload);
