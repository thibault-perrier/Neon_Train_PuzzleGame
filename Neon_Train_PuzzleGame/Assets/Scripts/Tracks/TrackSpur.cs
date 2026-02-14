public class TrackSpur : Track
{
    public override void DoTrackActivation()
    {
        foreach (TrackEndpoint endpoint in _endpoints)
        {
            endpoint.SwitchEndpoint();
        }
    }
}
