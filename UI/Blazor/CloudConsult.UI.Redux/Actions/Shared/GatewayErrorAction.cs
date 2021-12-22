namespace CloudConsult.UI.Redux.Actions.Shared
{
    public class GatewayErrorAction
    {
        public List<string> Errors { get; }
        public GatewayErrorAction(List<string> Errors)
        {
            this.Errors = Errors ?? new List<string> { "Invalid response from server" }; ;
        }
    }
}
