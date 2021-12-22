namespace CloudConsult.UI.Redux.States.Shared
{
    public record UIState
    {
        public bool Processing { get; init; }
        public bool Loading { get; init; }
        public List<string> Errors { get; init; }
        public List<string> Messages { get; init; }
    }
}
