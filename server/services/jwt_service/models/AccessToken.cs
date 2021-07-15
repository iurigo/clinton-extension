namespace server.services.jwt_service.models
{
  public class AccessToken
  {
    public string Token { get; set; }
    public string RefreshToken { get; set; }
  }
}