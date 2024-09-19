// using GameAnalyticsSDK;
// using UnityEngine;
//
// public class GATffRequest : MonoBehaviour,IGameAnalyticsATTListener
// {
//     void Start()
//     {
//         if(Application.platform == RuntimePlatform.IPhonePlayer)
//         {
//             // the next line will be commented when integrating Appodeal SDK due to conflicts
//             GameAnalytics.RequestTrackingAuthorization(this);
//         }
//         else
//         {
//             GameAnalytics.Initialize();
//         }
//     }
//
//     public void GameAnalyticsATTListenerNotDetermined()
//     {
//         GameAnalytics.Initialize();
//     }
//     public void GameAnalyticsATTListenerRestricted()
//     {
//         GameAnalytics.Initialize();
//     }
//     public void GameAnalyticsATTListenerDenied()
//     {
//         GameAnalytics.Initialize();
//     }
//     public void GameAnalyticsATTListenerAuthorized()
//     {
//         GameAnalytics.Initialize();
//     }
// }