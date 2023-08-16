
using core.architecture;
using core.constants;
using core.general.datamodels;
using core.managers.ui;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net.Http;
using System.Runtime.InteropServices;
using System.Text;
using UnityEngine;
using UnityEngine.Networking;

namespace core.general
{
    public class CustomUtils : Singleton<CustomUtils>
    {
        [SerializeField]
        private bool isInternetConnected;
        [SerializeField]
        private bool TimeMeOut;
        private Texture2D texture2D;
        public AudioClip audioClip;


        public Dictionary<DownloadedImagestypes, Dictionary<string, Texture2D>> DownloadedTextures = new Dictionary<DownloadedImagestypes, Dictionary<string, Texture2D>>();



        private void Start()
        {
            StartCoroutine(Networking_OnInternetDisconnected());
            //StartCoroutine(TimeMeoutNow());
            ServerConstants.InternetConnected = InternetConnectionStatus();

            /*DownloadedTextures.Add(DownloadedImagestypes.Dopez, new Dictionary<string, Texture2D>());
            DownloadedTextures.Add(DownloadedImagestypes.Leaderboard, new Dictionary<string, Texture2D>());
            DownloadedTextures.Add(DownloadedImagestypes.PlayerProfile, new Dictionary<string, Texture2D>());
            DownloadedTextures.Add(DownloadedImagestypes.UserProfile, new Dictionary<string, Texture2D>());
            DownloadedTextures.Add(DownloadedImagestypes.ProfileImages, new Dictionary<string, Texture2D>());*/
        }
        private void OnEnable()
        {
            
        }

        #region Internet Connectivity 

        public bool InternetConnectionStatus()
        {
            isInternetConnected = (Application.internetReachability == NetworkReachability.ReachableViaCarrierDataNetwork
                || Application.internetReachability == NetworkReachability.ReachableViaLocalAreaNetwork);
            return isInternetConnected;
        }
        private IEnumerator Networking_OnInternetDisconnected()
        {
            yield return new WaitForSeconds(3f);
            yield return new WaitUntil(() => !InternetConnectionStatus());
            UIManager.Instance.ShowPopup(PopupsType.NoInternet);
            //ServerConstants.ServerConnected = false;
            ServerConstants.UserLoggedIn = false;
            //Networking.Instance.Disconnect();
            //DestroyAllCoreManagers();
            //SceneManager.LoadScene(GameConstants.TimeOutInternetFailedScene);
        }
        #endregion

        #region Connection Timeout Connectivity
        
       
        public void DestroyAllCoreManagers(bool isTimeout = true)
        {
           
        }
        #endregion

        #region Download Data

        public void ReturnDownloadedTexture(string url, DownloadedImagestypes imageType, string imageID, Action<Texture2D> _DownloadSuccessfully, Action<string> _DonwloadFailed)
        {
            Debug.Log(url);
            if (DownloadedTextures[imageType].ContainsKey(imageID))
            {
                _DownloadSuccessfully(DownloadedTextures[imageType][imageID]);
                return;
            }


#if !UNITY_EDITOR
            StartCoroutine(DownloadTextureViaURL(url,imageType, imageID,
                DownloadSuccessfully =>
                {
                    _DownloadSuccessfully?.Invoke(DownloadSuccessfully); 
                },
                DonwloadFailed =>
                {
                    _DonwloadFailed?.Invoke(DonwloadFailed);
                }
            ));

#else
            DownloadTextureHttpRequest(url, imageType, imageID,
                DownloadSuccessfully =>
                {
                    Debug.Log($"Return Downloaded Texture Success: {DownloadSuccessfully}");
                    _DownloadSuccessfully?.Invoke(DownloadSuccessfully);
                },
                DonwloadFailed =>
                {

                    Debug.LogError($"Return Downloaded Texture Failed: {DonwloadFailed}");
                    _DonwloadFailed?.Invoke(DonwloadFailed);
                }
            );
#endif
        }
        public void ReturnDownloadedTexture(string url, DownloadedImagestypes imageType, string imageID)
        {
            StartCoroutine(DownloadTextureViaURL(url, imageType, imageID));
        }
        public void ReturnDownloadedAudio(string url)
        {
            StartCoroutine(DownloadAudioViaURL(url));
        }

        public IEnumerator DownloadTextureViaURL(string url, DownloadedImagestypes imageType, string imageID, Action<Texture2D> DownloadSuccessfully = null, Action<string> DonwloadFailed = null)
        {
            using (UnityWebRequest uwr = UnityWebRequestTexture.GetTexture(url))
            {
                yield return uwr.SendWebRequest();

                if (!string.IsNullOrEmpty(uwr.error))
                {
#if !UNITY_EDITOR
                    Debug.LogError(uwr.error);
#endif
                    DonwloadFailed?.Invoke(uwr.error);
                }
                else
                {
                    //Debug.Log(uwr);
                    //texture2D = new Texture2D(1000, 1000);
                    //byte[] bytes = ConvertStringToByte(uwr.downloadHandler.data.ToString());
                    //texture2D.LoadImage(bytes);
                    //texture2D.Apply();
                    //Debug.Log("Textrue Size: " + texture2D.texelSize);
                    //DownloadSuccessfully?.Invoke(texture2D);
                    texture2D = DownloadHandlerTexture.GetContent(uwr);
                    DownloadSuccessfully?.Invoke(texture2D);
                    if (DownloadedTextures[imageType].ContainsKey(imageID))
                    {
                        DownloadedTextures[imageType][imageID] = texture2D;
                    }
                    else
                    {
                        DownloadedTextures[imageType].Add(imageID, texture2D);
                    }

                }
            }
        }

        public async void DownloadTextureHttpRequest(string url, DownloadedImagestypes imageType, string imageID, Action<Texture2D> DownloadSuccessfully = null, Action<string> DonwloadFailed = null)
        {
            using (var httpClient = new HttpClient())
            {
                try
                {
                    var response = await httpClient.GetAsync(url);
                    response.EnsureSuccessStatusCode();

                    var imageBytes = await response.Content.ReadAsByteArrayAsync();

                    var texture = new Texture2D(639, 639);
                    texture.LoadImage(imageBytes);
                    DownloadSuccessfully?.Invoke(texture);
                    if (DownloadedTextures[imageType].ContainsKey(imageID))
                    {
                        DownloadedTextures[imageType][imageID] = texture;
                    }
                    else
                    {
                        DownloadedTextures[imageType].Add(imageID, texture);
                    }
                }
                catch (Exception ex)
                {
#if !UNITY_EDITOR
                    Debug.LogError(ex);
#endif
                    DonwloadFailed?.Invoke(ex.Message);
                }
            }

        }

        public IEnumerator DownloadAudioViaURL(string url)
        {
            Debug.Log($"Start downloading URL Song: {url}");
            using (UnityWebRequest uwr = UnityWebRequestMultimedia.GetAudioClip(url, AudioType.MPEG))
            {
                uwr.SetRequestHeader("Access-Control-Allow-Origin", url);
                yield return uwr.SendWebRequest();

                if (!string.IsNullOrEmpty(uwr.error))
                {
                    Debug.LogError(uwr.error);
                }
                else
                {
                    AudioClip clip = DownloadHandlerAudioClip.GetContent(uwr);
                    //clip.loadType = AudioClipLoadType.CompressedInMemory;
                   // SoundManager.Instance.UpdateSongInfo(url, clip);
                }
                uwr.Dispose();
            }
        }

        public void DestroyAllDownloadedTextures()
        {
            for (int i = 0; i < DownloadedTextures.Count; i++)
            {
                DestroyDownloadedTextures(DownloadedTextures.ElementAt(i).Key);
            }
        }

        public void DestroyDownloadedTextures(DownloadedImagestypes imageType)
        {
            for (int i = 0; i < DownloadedTextures[imageType].Count; i++)
            {
                DestroyImmediate(DownloadedTextures[imageType].ElementAt(i).Value);
            }
            DownloadedTextures[imageType].Clear();
        }

        public void DestroyDownloadedTexture(DownloadedImagestypes imageType, string imageId)
        {
            DestroyImmediate(DownloadedTextures[imageType][imageId]);
            DownloadedTextures[imageType].Remove(imageId);
        }

        #endregion

        #region Convert_String->Byte[]_&_Byte[]->String
        public string ConvertByteToString(byte[] _bytes)
        {
            return Encoding.UTF8.GetString(_bytes);
        }
        public byte[] ConvertStringToByte(string _string)
        {
            return Encoding.UTF8.GetBytes(_string);
        }
        #endregion

        #region TIME!



        public string TimeCalculatorTotalDaysLeft(long gameId)
        {
            DateTime dt2DateTime = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);
            dt2DateTime = dt2DateTime.AddMilliseconds(gameId).ToLocalTime();
            TimeSpan timeRemaining = dt2DateTime - DateTime.UtcNow;
            return timeRemaining.Days.ToString();
        }

        public string TimeCalculatorNumericDays(long c_time)
        {
            DateTime dtDateTime = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);
            dtDateTime = dtDateTime.AddMilliseconds(c_time).ToLocalTime();
            return dtDateTime.Day.ToString() + ConvertDays(dtDateTime) + " ";
        }

        public string TimeCalculatorAlphaDays(long c_time)
        {
            DateTime dtDateTime = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);
            dtDateTime = dtDateTime.AddMilliseconds(c_time).ToLocalTime();
            return dtDateTime.DayOfWeek.ToString();
        }

        public string TimeCalculatorNumericMonths(long c_time)
        {
            DateTime dtDateTime = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);
            dtDateTime = dtDateTime.AddMilliseconds(c_time).ToLocalTime();
            return dtDateTime.Month.ToString();
        }
        public string TimeCalculatorAlphaMonths(long c_time)
        {
            DateTime dtDateTime = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);
            dtDateTime = dtDateTime.AddMilliseconds(c_time).ToLocalTime();
            return DateTimeFormatInfo.CurrentInfo.GetAbbreviatedMonthName(dtDateTime.Month);
        }
        public string TimeCalculatorNumericYear(long c_time)
        {
            DateTime dtDateTime = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);
            dtDateTime = dtDateTime.AddMilliseconds(c_time).ToLocalTime();
            return dtDateTime.Year.ToString();
        }


        public TimeSpan CalculateTimeEndCountDown(long c_time)
        {
            DateTime endTime = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc); // start of unix time
            endTime = endTime.AddMilliseconds(c_time); // adding end time in start of unix to get end time in date time
            TimeSpan timeRemaining = endTime - DateTime.UtcNow;
            return timeRemaining;
        }

        public TimeSpan GetTime(long c_time)
        {
            TimeSpan timeRemaining = DateTime.UtcNow - (DateTime.UtcNow.AddSeconds(c_time * -1));
            return timeRemaining;
        }



        string ConvertDays(DateTime dtDateTime)
        {
            if (new[] { 11, 12, 13 }.Contains(dtDateTime.Day))
            {
                return "th";
            }
            else if (dtDateTime.Day % 10 == 1)
            {
                return "st";
            }
            else if (dtDateTime.Day % 10 == 2)
            {
                return "nd";
            }
            else if (dtDateTime.Day % 10 == 3)
            {
                return "rd";
            }
            else
            {
                return "th";
            }
        }
        #endregion

        #region Open New Window

        [DllImport("__Internal")]
        private static extern void OpenNewTab(string url);
        public void OpenNewTabURL(string url)
        {
#if !UNITY_EDITOR && UNITY_WEBGL
             OpenNewTab(url);
#elif UNITY_EDITOR
            Application.OpenURL(url);
#endif
        }
        #endregion

        #region Grid layout Group

        public int GetTotalPages(int generatedButtonCount, int layoutConstraintCount)
        {
            int totalPages = 0;
            totalPages = generatedButtonCount / layoutConstraintCount; // replace 14 with total generated enforcers count
            if (generatedButtonCount % layoutConstraintCount > 0)
            {
                totalPages++;
            }
            return totalPages;
        }

        public void NextPage(int pageNumber, int layoutConstraintCount/*, List<int> GeneratedButtons*/)
        {
            int startIndex = (pageNumber - 1) * layoutConstraintCount;
            for (int i = startIndex; i < startIndex + layoutConstraintCount; i++)
            {
                //print("disable" +i); //disable generated button
                if (i + layoutConstraintCount < 23)// replace number with list count
                {
                    //print("enable" + (i + layoutConstraintCount)); // enable generated button
                }
            }
        }

        public void PreviousPage(int pageNumber, int layoutConstraintCount/*, List<int> GeneratedButtons*/)
        {
            int startIndex = (pageNumber - 1) * layoutConstraintCount;
            for (int i = startIndex; i < startIndex + layoutConstraintCount; i++)
            {
                if (i < 23)// replace number with list count
                {
                    //print("disable" + i); //disable generated button
                }
                //print("enable" + (i - layoutConstraintCount)); // enable generated button
            }
        }




        #endregion

        #region Sorting Gameobjects

        public void AssignChildIndex<T>(List<T> list) where T : Component
        {
            for (int i = 0; i < list.Count; i++)
            {
                list[i].transform.SetSiblingIndex(i);
            }
        }

        #endregion

        #region Return Base Meta URLS

       /* public string ReturnBaseFacebookURL()
        {
            return DataManager.Instance.MetaData.Data.GameMetaConstants.RedirectingURL.BaseFacebookURL;
        }
        public string ReturnBaseInstagramURL()
        {
            return DataManager.Instance.MetaData.Data.GameMetaConstants.RedirectingURL.BaseInstagramURL;
        }
        public string ReturnBaseTwitterURL()
        {
            return DataManager.Instance.MetaData.Data.GameMetaConstants.RedirectingURL.BaseTwitterURL;
        }
        public string ReturnBaseTwitchURL()
        {
            return DataManager.Instance.MetaData.Data.GameMetaConstants.RedirectingURL.BaseTwitchURL;
        }
        public string ReturnMarketPlaceURL()
        {
            return DataManager.Instance.MetaData.Data.GameMetaConstants.RedirectingURL.MarketPlaceURL;
        }*/
        #endregion

        #region Seek Time
        public string SeekTime(double time)
        {
            TimeSpan currentTime = TimeSpan.FromSeconds(time);
            return currentTime.Hours switch
            {
                > 0 => currentTime.ToString("hh':'mm':'ss"),
                _ => currentTime.ToString("mm':'ss")
            };
        }
        #endregion

        #region Currency Utils
        public string ScoreShow(string Score)
        {
            string result = "";
            float score2 = 0;
            if (float.TryParse(Score, out float Scor))
            {
                int i;
                score2 = MathF.Floor(Scor);

                for (i = 0; i < GameConstants.ScoreNames.Length; i++)
                {
                    if (Scor < 900)
                    {
                        break;
                    }
                    else
                    {
                        Scor = Mathf.Floor(Scor / 100f) / 10f;
                    }
                }

                if (Convert.ToInt64(score2) <= 99999)
                {
                    result = Convert.ToInt64(score2).ToString("N0");
                }
                else
                {
                    if (Scor == Mathf.Floor(Scor))
                    {
                        result = Scor.ToString() + GameConstants.ScoreNames[i];
                    }
                    else
                    {
                        result = Scor.ToString("F1") + GameConstants.ScoreNames[i];
                    }
                }
            }
            return result;
        }
        #endregion

        #region String Replace and Lower
        public string RemoveSpace_Lower(string value)
        {
            StringBuilder stringBuilder = new StringBuilder();
            stringBuilder.Append(value).Replace(" ", "");
            return stringBuilder.ToString().ToLower();
        }

        #endregion

        #region float equality check

        public bool EqualityCheckOnFloats(double x, double y, double tolerance = 1e-10)
        {
            var diff = Math.Abs(x - y);
            return diff <= tolerance || diff <= Math.Max(Math.Abs(x), Math.Abs(y)) * tolerance;
        }

        #endregion
        private void OnDisable()
        {
            DeInitialize();
        }
        protected override void DeInitialize()
        {
            base.DeInitialize();
        }
        private void OnDestroy()
        {
            DeInitialize();
        }
    }
    #region Texture -> Sprite && Sprite -> Texture
    public static class TextureSpriteUtils
    {
        public static Sprite ConvertToSprite(this Texture2D texture)
        {
            return Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), Vector2.zero);
        }

        public static Texture2D ConvertSpriteToTexture(Sprite sprite)
        {
            try
            {
                if (sprite.rect.width != sprite.texture.width)
                {
                    Texture2D newText = new Texture2D((int)sprite.rect.width, (int)sprite.rect.height);
                    Color[] colors = newText.GetPixels();
                    Color[] newColors = sprite.texture.GetPixels((int)System.Math.Ceiling(sprite.textureRect.x),
                                                                 (int)System.Math.Ceiling(sprite.textureRect.y),
                                                                 (int)System.Math.Ceiling(sprite.textureRect.width),
                                                                 (int)System.Math.Ceiling(sprite.textureRect.height));
                    Debug.Log(colors.Length + "_" + newColors.Length);

                    newText.SetPixels(newColors);
                    newText.Apply();
                    return duplicateTexture(newText);
                }
                else
                    return duplicateTexture(sprite.texture);
            }
            catch
            {
                return duplicateTexture(sprite.texture);
            }
        }
        public static Texture2D duplicateTexture(Texture2D source)
        {
            RenderTexture renderTex = RenderTexture.GetTemporary(source.width, source.height, 0, RenderTextureFormat.Default, RenderTextureReadWrite.Linear);
            Graphics.Blit(source, renderTex);
            RenderTexture previous = RenderTexture.active;
            RenderTexture.active = renderTex;
            Texture2D readableText = new Texture2D(source.width, source.height);
            readableText.ReadPixels(new Rect(0, 0, renderTex.width, renderTex.height), 0, 0);
            readableText.Apply();
            RenderTexture.active = previous;
            RenderTexture.ReleaseTemporary(renderTex);
            return readableText;
        }
        public static string RemoveSpecialCharacters(this string str)
        {
            StringBuilder sb = new StringBuilder();
            foreach (char c in str)
            {
                if ((c >= '0' && c <= '9') || (c >= 'A' && c <= 'Z') || (c >= 'a' && c <= 'z') || c == '.' || c == '_')
                {
                    sb.Append(c);
                }
            }
            return sb.ToString();
        }
    }
    #endregion
}


