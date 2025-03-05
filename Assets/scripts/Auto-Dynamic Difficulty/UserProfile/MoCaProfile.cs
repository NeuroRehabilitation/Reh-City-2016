using System;
using Assets.scripts.Models;

namespace Assets.scripts.UserProfile
{
    public class MoCaProfile
    {
        public readonly int MoCaAtt;
        public readonly int MoCaMem;
        public readonly int MoCaExFunc;
        public readonly int MoCaLang;
        public readonly int MoCaDiff;
        public string UserId;
        public Model UserModel;

        public MoCaProfile(string id, int vs, int nam, int att, int lang, int abstr, int dr, int orient, int edu)
        {
            UserId = id;
            MoCaAtt = att;
            MoCaMem = dr + orient;
            MoCaExFunc = vs + abstr;
            MoCaLang = lang + nam;
            MoCaDiff = MoCaAtt + MoCaMem + MoCaExFunc + MoCaLang + edu;
            if (MoCaDiff > 30)
                MoCaDiff = 30;
        }

        public Model CalculateModelFromMoCa()
        {
            var att =  (float)Math.Round(MoCaAtt * 10.0f / 6.0f, 1, MidpointRounding.AwayFromZero);
            var mem = (float)Math.Round(MoCaMem * 10.0f / 11.0f, 1, MidpointRounding.AwayFromZero);
            var exec = (float)Math.Round(MoCaExFunc * 10.0f/ 7.0f, 1, MidpointRounding.AwayFromZero);
            var lang = (float)Math.Round(MoCaLang * 10.0f / 6.0f, 1, MidpointRounding.AwayFromZero);
            var dif = (float)Math.Round(MoCaDiff * 10.0f / 30.0f, 1, MidpointRounding.AwayFromZero);

            att = (float)Math.Round(att / 0.5f, 0) * 0.5f;
            mem = (float)Math.Round(mem / 0.5f, 0) * 0.5f;
            exec = (float)Math.Round(exec / 0.5f, 0) * 0.5f;
            lang = (float)Math.Round(lang / 0.5f, 0) * 0.5f;
            dif = (float)Math.Round(dif / 0.5f, 0) * 0.5f;

            if (att < 1)
                att = 1;

            if (mem < 1)
                mem = 1;

            if (exec < 1)
                exec = 1;

            if (lang < 1)
                lang = 1;

            if (dif < 1)
                dif = 1;

            UserModel = new Model(att, mem, exec, lang, dif);

            return UserModel;
        }
    }
}
