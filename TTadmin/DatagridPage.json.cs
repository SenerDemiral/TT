using System;
using Starcounter;
using Starcounter.Templates;

namespace TTadmin
{
    partial class DatagridPage : Json
    {
        bool reading = false;

        public void RefreshPets()
        {
            reading = true;
            Pets.Clear();

            var trns = Db.SQL<TTDB.Turnuva>("SELECT tt FROM Turnuva tt");
            DatagridPagePetsElementJson pet;
            Fetching = true;
            foreach (var trn in trns) {
                pet = this.Pets.Add();
                pet.Name = trn.Ad;
                pet.Kind = string.Format("{0:dd.MM.yy}", trn.Trh);
                pet.ID = trn.GetObjectID();
                pet.Sil = false;

            }
            Fetching = false;
            reading = false;
        }

        protected override void OnData()
        {
            base.OnData();

            RefreshPets();
        }

        protected override void HasChanged(TValue property) // Insert edince calisiyor
        {
        }

        void Handle(Input.highlightedRow action)
        {
            //var aaa = this.GetObjectID();
        }

        void Handle(Input.dilara action)
        {
            //var aaa = this.GetObjectID();
        }
        void Handle(Input.AddPet action)
        {
            reading = true;
            var p = Pets.Add();
            p.Name = "";
            p.Kind = string.Format("{0:dd.MM.yy}", DateTime.Today);
            reading = false;
        }

        void Handle(Input.Save action)
        {
            reading = true;
            foreach (var pet in Pets) {
                var aaa = pet.ChangeLog;
                if (pet.Degisti) {
                    if (!string.IsNullOrEmpty(pet.ID)) {
                        var trnObj = (TTDB.Turnuva)DbHelper.FromID(DbHelper.Base64DecodeObjectID(pet.ID));
                        trnObj.Ad = pet.Name;
                        trnObj.Trh = DateTime.ParseExact(pet.Kind, "dd.MM.yy", System.Globalization.CultureInfo.InvariantCulture);
                    } else {
                        var t = new TTDB.Turnuva();
                        t.Ad = pet.Name;
                        if (string.IsNullOrEmpty(pet.Kind))
                            t.Trh = DateTime.Now;
                    }

                }
            }
            for (int i = 0; i < Pets.Count; i++)
                Pets[i].Degisti = false;
            
            Transaction.Commit();
            reading = false;
            //var trn = (TTDB.Turnuva)DbHelper.FromID(DbHelper.Base64DecodeObjectID(Pets[0].ID));
            // var trn = (TTDB.Turnuva)DbHelper.FromID(DbHelper.Base64ForUrlDecode(Pets[0].ID));
            //trn.Ad = Pets[0].Name;
            //Transaction.Commit();
        }


        [DatagridPage_json.Pets]
        partial class DatagridPagePetsElementJson : Json
        {
            protected override void OnData()
            {
                base.OnData();
            }
            
            protected override void HasChanged(TValue property)
            {
                var parent = (DatagridPage)this.Root;
                
                //if (!parent.Fetching && property.PropertyName != "Degisti")
                //if (!parent.reading && property.PropertyName != "Degisti")
                //    Degisti = true;
            }

            public string CalculatedSound {
                get {
                    switch (Kind.ToLower()) {
                        case "dog":
                            return "Woof";

                        case "cat":
                            return "Meow";

                        case "rabbit":
                            return "Jump";

                        case "hamster":
                            return "Squeak";

                        default:
                            return "";
                    }
                }
            }

            void Handle(Input.Name inp)
            {
                var a = inp.OldValue;
                var b = inp.Value;
                var c = inp.Template;
                //Degisti = true;
            }
        }
    }
}
