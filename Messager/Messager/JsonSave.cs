using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Messager
{
    public class JsonSave
    {

        static JsonSave()
        {

            if (!Directory.Exists("Save")) Directory.CreateDirectory("Save");

            if (!File.Exists(userFileName)) File.Create(userFileName).Close();
            if (!File.Exists(dialogFileName)) File.Create(dialogFileName).Close();

            using (var f = new StreamReader(userFileName))
                users = JsonConvert.DeserializeObject<List<SaveObject>>(f.ReadToEnd()) ?? new List<SaveObject>();

            using (var f = new StreamReader(dialogFileName))
                dialogs = JsonConvert.DeserializeObject<List<SaveObject>>(f.ReadToEnd()) ?? new List<SaveObject>();

        }

        const string userFileName = "Save\\users.json";
        const string dialogFileName = "Save\\dialogs.json";

        static List<SaveObject> users;
        static List<SaveObject> dialogs;

        static int id;

        public static void Auth(int id_first, int id_second)
        {

            var res = users.Find(u => u.id_first == id_first && u.id_second == id_second);

            if (res == null) 
            {

                users.Add(new SaveObject()
                {

                    id = users.Count + 1,
                    id_first = id_first,
                    id_second = id_second

                });
                SaveUsers();

                id = users.Count;

                return;

            }
            else id = res.id;

        }

        public static List<SaveObject> GetDialogs() => dialogs.FindAll(d => d.id == id);

        public static bool SaveDialog(int id_first, int id_second)
        {

            if (dialogs.FindAll(d => d.id == id).Find(d => d.id_first == id_first || d.id_second == id_second) != null) return false;

            dialogs.Add(new SaveObject()
            {

                id = id,
                id_first = id_first,
                id_second = id_second

            });

            SaveDialogs();
            return true;

        }

        static void SaveUsers()
        {

            if (users == null) users = new List<SaveObject>();

            using (var f = new StreamWriter(userFileName, false))
                f.Write(JsonConvert.SerializeObject(users));

        }

        static void SaveDialogs()
        {

            if (dialogs == null) dialogs = new List<SaveObject>();

            using (var f = new StreamWriter(dialogFileName, false))
                f.Write(JsonConvert.SerializeObject(dialogs));

        }

        [Serializable]
        public class SaveObject
        {

            public int id;
            public int id_first;
            public int id_second;

        }

    }
}
