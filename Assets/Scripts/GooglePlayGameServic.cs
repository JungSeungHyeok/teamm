using GooglePlayGames.BasicApi;
using GooglePlayGames;
using UnityEngine;

public class GooglePlayGameServic : MonoBehaviour
{
    public void Start()
    {
        PlayGamesPlatform.Instance.Authenticate(ProcessAuthentication);
    }

    internal void ProcessAuthentication(SignInStatus status)
    {
        Debug.Log(status);

        if (status == SignInStatus.Success)
        {
        }
        else
        {
            PlayGamesPlatform.Instance.ManuallyAuthenticate(ProcessAuthentication);
        }
    }








}
