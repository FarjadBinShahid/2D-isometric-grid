
using System;
using core.architecture;
using core.general.datamodels;

namespace core.managers.data
{
    public class DataManager : Singleton<DataManager>
    {
        public MetaData MetaData;

        public static event Action<MetaData> onMetaLoaded = null;

        public void PostLogin()
        {


           /* Networking.Instance.GetMetaData(
                (meta) =>
                {
                    var jObject = JObject.Parse(meta);
                    MetaData = JsonConvert.DeserializeObject<MetaData>(jObject["metadata"]["metaData"].ToString());
                    UiManager.Instance.ShowView(ViewsType.MainView);
                },
                (fail) =>
                {
                    Debug.LogError("Failed to load meta \n" + fail);
                    UiManager.Instance.ShowPopup(PopupsType.GeneralError, "Failed to load meta", fail); // TODO : make a saperate popup with refresh tab button
                });   */
        }
    }
}