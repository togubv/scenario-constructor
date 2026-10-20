namespace SC
{
    public readonly struct GameEvent
    {
        public readonly EventType eventType;
        public readonly string id;

        public GameEvent(EventType eventType, string id)
        {
            this.eventType = eventType;
            this.id = id;
        }
    }
}
