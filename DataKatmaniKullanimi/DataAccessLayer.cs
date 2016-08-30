using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.SqlClient;

namespace DataKatmaniKullanimi
{
    //eklediğiniz projelerin namespace ile değiştiriniz.
    public class DataAccessLayer
    {
        public SqlConnection Connect()
        {
            SqlConnection con = new SqlConnection("server=.\\SQLEXPRESS; database=northwnd; integrated security=true");
            con.Open();
            return con;
        }
        //farklı db lerle çalışıyorsak
        public SqlConnection Connect(string dbName)
        {
            SqlConnection con = new SqlConnection(string.Format("server=.; database={0}; integrated security=true",dbName));
            con.Open();
            return con;
        }


        ///////////////////////////////////
        //Parametre bildirimlerinin oluşturulması
        ///////////////////////////////////

        void AddParamaters(SqlCommand command, List<Parametreler> list)
        {
            foreach (var parametre in list)
            {
                command.Parameters.AddWithValue(parametre.Name, parametre.Value);
            }
        }

        void AddParamatersWithType(SqlCommand command, List<Parametreler> list)
        {
            SqlParameter p;
            foreach (var parametre in list)
            {
                p = new SqlParameter(parametre.Name, parametre.Type);
                p.Value = parametre.Value;
                command.Parameters.Add(p);
            }
        }


        //insert-update-delete
        public int RunASqlStatement(string statement)
        {
            SqlCommand command = new SqlCommand(statement, Connect());

            return command.ExecuteNonQuery();
        }

        public int RunASqlStatement(string statement, List<Parametreler> list)
        {
            SqlCommand command = new SqlCommand(statement, Connect());
            AddParamaters(command, list);//sorguda tanımlanan @p1, @p2, .... gibi parametre isimlerini ve bu isimlere karşılık gelecek değerleri biraraya getir. 
            return command.ExecuteNonQuery();
        }

        public int RunASqlStatementWithType(string statement, List<Parametreler> list)
        {
            SqlCommand command = new SqlCommand(statement, Connect());
            AddParamatersWithType(command, list);//sorguda tanımlanan @p1, @p2, .... gibi parametre isimlerini ve bu isimlere karşılık gelecek değerleri biraraya getir. 
            return command.ExecuteNonQuery();
        }

        public int RunASqlStatement(string statement, string dbName)
        {
            SqlCommand command = new SqlCommand(statement, Connect(dbName));

            return command.ExecuteNonQuery();
        }
        //overload edilebilir...



        ////////////////////////////
        //select işlemleri
        ////////////////////////////

        public DataTable GetDataTable(string statement)
        {
            DataTable dt = new DataTable();
            SqlDataAdapter adap = new SqlDataAdapter(statement, Connect());
            adap.Fill(dt);

            return dt;//0 satırlık ya da data içeren bir tablo geri döner.
        }

        public DataTable GetDataTable(string statement, List<Parametreler> list)
        {
            DataTable dt = new DataTable();
            SqlDataAdapter adap = new SqlDataAdapter(statement, Connect());
            //adaptor statement ı selectcommand in text i olarak kabul eder.
            AddParamaters(adap.SelectCommand, list);
            adap.Fill(dt);

            return dt;//0 satırlık ya da data içeren bir tablo geri döner.
        }


        public DataRow GetDataRow(string statement)
        {
            DataTable dt = GetDataTable(statement);

            if (dt.Rows.Count != 0)//sorgu sonucunda 1 veya + data satırı elde eldilmişse
                return dt.Rows[0];
            else
                return null;//hiç bir sonuç ortaya çıkmıyorsa null return edilebilir.
        }

        public DataRow GetDataRow(string statement, List<Parametreler> list)
        {
            DataTable dt = GetDataTable(statement, list);

            if (dt.Rows.Count != 0)//sorgu sonucunda 1 veya + data satırı elde eldilmişse
                return dt.Rows[0];
            else
                return null;//hiç bir sonuç ortaya çıkmıyorsa null return edilebilir.
        }

        public string GetScalar(string statement)
        {
            SqlCommand command = new SqlCommand(statement, Connect());
            return  command.ExecuteScalar().ToString();
        }

        public string GetScalar(string statement, List<Parametreler> list)
        {
            SqlCommand command = new SqlCommand(statement, Connect());
            AddParamaters(command, list);
            return command.ExecuteScalar().ToString();
        }

        //ondalıklı sayılar

        public string ToCurrencyDB(string number)
        {
            return number.Replace(",", ".");
            //***sistem noktalı sayıları , ile yazıldığında kabul eder.
            //db ye gönderim de noktalı sayılar sadece noktayla yazılmalı
        }

        public string ToCurrencyCode(string number)
        {
            return number.Replace(".", ",");
            //db den gelecek noktalı sayıları sisteme , ile aktarsın
        }


    }


    public class Parametreler
    {
        public string Name { get; set; }
        public object Value { get; set; }
        public SqlDbType? Type { get; set; }//SqlDbType non-nullable (yani null değer içeremez) bu nedenle tanımda SqlDbType? 


        public Parametreler()
        {
            Name = null;
            Value = null;
            Type = null;
        }
    }
}
