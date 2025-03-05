using System.Globalization;
using Assets.scripts.Models;
using Assets.scripts.Settings;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.scripts.UserProfile
{
    public class ModelProfile : MonoBehaviour
    {
        public Text AttValue, MemValue, ExecFuncValue, LangValue, DifValue, UserId;
        public Text AttText, MemText, ExecFuncText, LangText, DifText;
        public GameObject AttBar, MemBar, ExecFuncBar, LangBar, DiffBar;

        private static string _attValue = "5", _memValue = "5", _execFuncValue = "5", _langValue = "5", _difValue = "5", _userId = "";

        private Color _attColor = Color.green;
        private Color _memColor = Color.green;
        private Color _execColor = Color.green;
        private Color _langColor = Color.green;
        private Color _difColor = Color.green;

        private Vector2 _barSize;

        private void Start()
        {
            //saves the initial size of one bar (all have the same size so only is necessary to get one)
            _barSize = AttBar.GetComponent<RectTransform>().offsetMax;
        }

        private void Update ()
        {
            if(Application.loadedLevel == 0 || Application.loadedLevelName == "GameOver")
                UpdateBars();
        }
        
        /// <summary>
        /// Receives MoCa values to create a new profile
        /// </summary>
        /// <param name="id">user id</param>
        /// <param name="att">moca attention value</param>
        /// <param name="mem">moca memory value</param>
        /// <param name="exec">moca exec. functions value</param>
        /// <param name="lang">moca language value</param>
        /// <param name="dif">moca difficulty value</param>
        public static void UpdateMoCAProfile(string id, int vs, int nam, int att, int lang, int abstr, int dr, int orient, int edu)
        {
            var moCa = new MoCaProfile(id, vs, nam, att, lang, abstr, dr, orient, edu);
            var profile = moCa.CalculateModelFromMoCa();

            _attValue = profile.Attention.ToString(CultureInfo.InvariantCulture);
            _memValue = profile.Memory.ToString(CultureInfo.InvariantCulture);
            _execFuncValue = profile.ExFunctions.ToString(CultureInfo.InvariantCulture);
            _langValue = profile.Language.ToString(CultureInfo.InvariantCulture);
            _difValue = profile.Difficulty.ToString(CultureInfo.InvariantCulture);
            _userId = id;
        }

        public static void UpdateModelProfile(string id, float att, float mem, float exec, float lang, float diff)
        {
            var profile = new Model(att, mem, exec, lang, diff);

            _attValue = profile.Attention.ToString(CultureInfo.InvariantCulture);
            _memValue = profile.Memory.ToString(CultureInfo.InvariantCulture);
            _execFuncValue = profile.ExFunctions.ToString(CultureInfo.InvariantCulture);
            _langValue = profile.Language.ToString(CultureInfo.InvariantCulture);
            _difValue = profile.Difficulty.ToString(CultureInfo.InvariantCulture);
            _userId = id;
        }

        private void UpdateBars()
        {
            AttValue.text = _attValue;
            MemValue.text = _memValue;
            ExecFuncValue.text = _execFuncValue;
            LangValue.text = _langValue;
            DifValue.text = _difValue;
            UserId.text = _userId;

            AttText.text = Language.attention;
            MemText.text = Language.memory;
            ExecFuncText.text = Language.execFunc;
            LangText.text = Language.lang;
            DifText.text = Language.difficulty;

            //changes the color of the bars according to values
            _attColor = Color.Lerp(Color.green, Color.red, float.Parse(_attValue)/10.0f);
            _memColor = Color.Lerp(Color.green, Color.red, float.Parse(_memValue) / 10.0f);
            _execColor = Color.Lerp(Color.green, Color.red, float.Parse(_execFuncValue) / 10.0f);
            _langColor = Color.Lerp(Color.green, Color.red, float.Parse(_langValue) / 10.0f);
            _difColor = Color.Lerp(Color.green, Color.red, float.Parse(_difValue) / 10.0f);

            AttBar.GetComponent<Image>().color = _attColor;
            MemBar.GetComponent<Image>().color = _memColor;
            ExecFuncBar.GetComponent<Image>().color = _execColor;
            LangBar.GetComponent<Image>().color = _langColor;
            DiffBar.GetComponent<Image>().color = _difColor;

            //changes the size of the bars according to values
            AttBar.GetComponent<RectTransform>().offsetMax = new Vector2(- (100 - float.Parse(_attValue)*10),_barSize.y);
            MemBar.GetComponent<RectTransform>().offsetMax = new Vector2(-(100 - float.Parse(_memValue) * 10), _barSize.y);
            ExecFuncBar.GetComponent<RectTransform>().offsetMax = new Vector2(-(100 - float.Parse(_execFuncValue) * 10), _barSize.y);
            LangBar.GetComponent<RectTransform>().offsetMax = new Vector2(-(100 - float.Parse(_langValue) * 10), _barSize.y);
            DiffBar.GetComponent<RectTransform>().offsetMax = new Vector2(-(100 - float.Parse(_difValue) * 10), _barSize.y);
        }
    }
}
