using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SqlClient;
using Login_Ekranı.Data;
using System.Data.Entity;

namespace Login_Ekranı
{
    public partial class Form1 : Form
    {
        LoginDbEntities db = new LoginDbEntities();
        public Form1()
        {
            InitializeComponent();
        }
        int loginError = 0;
        private void btnLogin_Click(object sender, EventArgs e)
        {
            if (txtUserName.Text == "" && txtPassword.Text == "")
            {
                MessageBox.Show("Kullanıcı Adı ve Şifre Boş Bırakılamaz.", "UYARI", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            else if (txtUserName.Text == "" && txtPassword.Text != "")
            {
                MessageBox.Show("Kullanıcı Adı Boş Bırakılamaz.", "UYARI", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            else if (txtUserName.Text != "" && txtPassword.Text == "")
            {
                MessageBox.Show("Şifre Boş Bırakılamaz.", "UYARI", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            else if (txtUserName.Text.Length < 2)//MaxLength 20 properties kısmından belirlenmiştir.
            {
                MessageBox.Show("Username bilgisini giriniz.", "UYARI", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            //4 - Password alanına her türlü string ifade girilebilir.En fazla 8 en az 5 karakter olmalıdır. 5 karakterden az bilgi girildiğinde "Password bilgisini giriniz" mesajı görüntülenecek
            else if (txtPassword.Text.Length < 5)//MaxLength 8 properties kısmından belirlenmiştir.
            {
                MessageBox.Show("Password bilgisini giriniz.", "UYARI", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            var kullanici = db.Uyeler.Where(x => x.kullanici_adi == txtUserName.Text).FirstOrDefault();

            if (kullanici == null)
            {
                lblMessage.Text = $"'{txtUserName.Text}' Sistemde Bulunmadı";

            }

            else
            {
                kullanici = db.Uyeler.Where(x => x.kullanici_adi == txtUserName.Text && x.kullanici_sifre == txtPassword.Text && x.IsActive == true).FirstOrDefault();
                if (kullanici == null)
                {
                    if (loginError < 3)
                    {
                        loginError++;
                    }
                    else//3'ten fazla girilirse hesap bloke edilecektir.
                    {
                        MessageBox.Show("Hesap Bloke Edilmiştir", "UYARI", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        var uyeBloke = db.Uyeler.Where(x => x.kullanici_adi == txtUserName.Text).FirstOrDefault();
                        uyeBloke.IsActive = false;
                        db.Entry(uyeBloke).State = EntityState.Modified;
                        db.SaveChanges();
                    }
                }
                else
                {
                    MessageBox.Show("Giriş Başarılı", "BİLGİ", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
        }
        private void txtUserName_KeyPress(object sender, KeyPressEventArgs e)
        {
            //3- UserName alanına harf ve rakam girilebilir ama noktalama işareti girilemez. En fazla 20 en az 2 karakter olmalıdr. 2 karakterden az bilgi girildiğinde "Username bilgisini giriniz" mesajı görüntülenecek.
            e.Handled = Char.IsPunctuation(e.KeyChar);//Textboxta özel karakter girilmesini önler.
        }
    }
}
