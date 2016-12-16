using System;
using Starcounter;
using Starcounter.Templates;
using Starcounter.XSON.Templates.Factory;

namespace TTadmin
{
    partial class DatagridPage : Json
    {
        bool reading = false;

        public void RefreshTurnuva()
        {
            reading = true;
            Trns.Clear();

            Sec.Insert(0, new Json(@"'one ·1'"));
            Sec.Insert(1, new Json(@"'two ·2'"));
            Sec.Insert(2, new Json(@"'three ·3'"));
            Sec.Add(new Json(@"'five ·5'"));

            var trns = Db.SQL<TTDB.Turnuva>("SELECT tt FROM Turnuva tt");
            DatagridPageTrnsElementJson pet;
            Fetching = true;
            foreach (var trn in trns) {
                pet = this.Trns.Add();
                pet.Ad = trn.Ad;
                pet.Tarih = string.Format("{0:dd.MM.yy}", trn.Trh);
                pet.ID = trn.GetObjectID();
                pet.Sil = false;

            }
            Fetching = false;
            reading = false;
        }

        protected override void OnData()
        {
            base.OnData();

            RefreshTurnuva();
        }

        protected override void HasChanged(TValue property) // Insert edince calisiyor
        {
        }

        void Handle(Input.highlightedRow action)
        {
            //var aaa = this.GetObjectID();
        }

        void Handle(Input.highlightedRowID action)
        {
            //var aaa = this.GetObjectID();
            //if (action.Value == "Vu")
            //    Sec.Add(new Json(@"'four ·4'"));
        }
        void Handle(Input.Insert action)
        {
            reading = true;
            var p = Trns.Add();
            p.Ad = "";
            p.Tarih = string.Format("{0:dd.MM.yy}", DateTime.Today);
            reading = false;
        }

        void Handle(Input.Refresh action)
        {
            RefreshTurnuva();
        }

        void Handle(Input.Save action)
        {
            reading = true;
            bool deleteVar = false;
            foreach (var pet in Trns) {
                var aaa = pet.ChangeLog;
                if (pet.Degisti) {
                    if (!string.IsNullOrEmpty(pet.ID)) {
                        var trnObj = (TTDB.Turnuva)DbHelper.FromID(DbHelper.Base64DecodeObjectID(pet.ID));
                        if (pet.Sil) {
                            trnObj.Delete();
                            
                            deleteVar = true;
                        }
                        else {
                            trnObj.Ad = pet.Ad;
                            trnObj.Trh = DateTime.ParseExact(pet.Tarih, "dd.MM.yy", System.Globalization.CultureInfo.InvariantCulture);
                        }
                    } else {
                            var t = new TTDB.Turnuva();
                            t.Ad = pet.Ad;
                            if (string.IsNullOrEmpty(pet.Tarih))
                                t.Trh = DateTime.Now;
                    }

                }
            }
            Transaction.Commit();

            if (deleteVar)
                RefreshTurnuva();
            else {
                for (int i = 0; i < Trns.Count; i++)
                    Trns[i].Degisti = false;
            }
            reading = false;
            //var trn = (TTDB.Turnuva)DbHelper.FromID(DbHelper.Base64DecodeObjectID(Pets[0].ID));
            // var trn = (TTDB.Turnuva)DbHelper.FromID(DbHelper.Base64ForUrlDecode(Pets[0].ID));
            //trn.Ad = Pets[0].Name;
            //Transaction.Commit();
        }


        [DatagridPage_json.Trns]
        partial class DatagridPageTrnsElementJson : Json
        {
            protected override void OnData()
            {
                base.OnData();
            }
            
            protected override void HasChanged(TValue property)
            {
                var parent = (DatagridPage)this.Root;
                
                //if (!parent.Fetching && property.PropertyName != "Degisti")
                if (!parent.reading && property.PropertyName != "Degisti")
                    Degisti = true;
            }

            public string CalculatedSound {
                get {
                    switch (Tarih.ToLower()) {
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

            void Handle(Input.Ad inp)
            {
                var a = inp.OldValue;
                var b = inp.Value;
                var c = inp.Template;
                //Degisti = true;
            }
        }
    }
}
