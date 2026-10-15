<div align="center">

<!-- Kapak görselini screenshots/01-kapak.png konumuna ekleyin. -->
<img width="1200" alt="Nexora - Mesajlaşma Platformu" src="screenshots/01-kapak.png" />




✉️ Nexora
ASP.NET Core MVC & Identity Based Messaging Platform
Nexora, kullanıcıların güvenli bir şekilde mesajlaşabildiği, mesajlarını düzenleyebildiği ve hesaplarını yönetebildiği; kullanıcı ve yönetici panellerine sahip web tabanlı bir mesajlaşma uygulamasıdır.











</div>

✨ Proje Hakkında
Nexora, ASP.NET Core MVC mimarisiyle geliştirilmiş bir mesajlaşma sistemidir. Kullanıcılar platforma kayıt olup giriş yaptıktan sonra sistemde kayıtlı diğer kullanıcılara mesaj gönderebilir, gelen ve gönderilen mesajlarını görüntüleyebilir, mesajları yanıtlayabilir ve kişisel mesaj kutularını yönetebilir.
Uygulamada ASP.NET Core Identity ile kimlik doğrulama ve rol yönetimi; Entity Framework Core ve SQL Server ile veri yönetimi sağlanmaktadır. Kullanıcı arayüzünde Nexora'ya özel modern bir tasarım, yönetici tarafında ise ayrı bir Admin Paneli bulunmaktadır.
🔐 Kayıt Olma ve Giriş Yapma
Sisteme kayıt olan kullanıcılar kendi hesaplarıyla giriş yapabilir. Identity altyapısı üzerinden parola doğrulama, benzersiz e-posta kontrolü ve yetkilendirme işlemleri gerçekleştirilir.
- Kullanıcı kayıt ve giriş ekranları
- Identity ile parola ve hesap yönetimi
- Kullanıcı ve Admin rollerine göre yönlendirme
- Oturum açma ve güvenli çıkış yapma
- Şifremi Unuttum bilgilendirme ekranı
Not: Şifremi Unuttum ekranı hazırlanmıştır; bu projede e-posta gönderimi ve bağlantı üzerinden gerçek parola sıfırlama aktif değildir.

<table>
<tr>
<td width="50%" align="center">

Giriş Yap
<img alt="Nexora giriş ekranı" src="screenshots/02-login.png" />

</td>
<td width="50%" align="center">

Kayıt Ol
<img alt="Nexora kayıt ekranı" src="screenshots/03-register.png" />

</td>
</tr>
</table>

📥 Gelen Kutusu ve Mesaj Yönetimi
Kullanıcılar kendilerine gelen mesajları Gelen Kutusu ekranında görüntüleyebilir. Mesajların göndereni, konusu, içeriğinin önizlemesi ve gönderim tarihi listelenir.
- Kayıtlı kullanıcıya yeni mesaj gönderme
- Gelen ve gönderilen mesajları ayrı ekranlarda görüntüleme
- Mesaj detayını açma ve yanıtlama
- Açılan mesajı otomatik olarak okundu işaretleme
- Mesajı yeniden okunmadı işaretleme
- Mesajları yıldızlama ve yıldızlı mesajları listeleme
- Gönderilen mesajın alıcı tarafından okunup okunmadığını görüntüleme
- Mesajları çöp kutusuna taşıma ve geri yükleme
Gelen Kutusu
<img width="1200" alt="Nexora gelen kutusu" src="screenshots/04-gelen-kutusu.png" />

Mesaj Detayı
<img width="1200" alt="Nexora mesaj detayı" src="screenshots/05-mesaj-detay.png" />

Yeni Mesaj Oluşturma
<img width="1200" alt="Nexora yeni mesaj oluşturma" src="screenshots/06-yeni-mesaj.png" />

Gönderilen Mesajlar
<img width="1200" alt="Nexora gönderilen mesajlar" src="screenshots/07-gonderilenler.png" />

📝 Taslak Yönetimi
Henüz gönderilmeye hazır olmayan mesajlar taslak olarak kaydedilebilir. Kullanıcılar daha sonra taslaklarını açarak düzenlemeye devam edebilir.
Taslak işlemleri arasında kaydetme, listeleme, düzenleme, gönderme ve silme bulunmaktadır. Bir taslak gönderildiğinde mesaj gönderilenler bölümüne aktarılır.
<img width="1200" alt="Nexora taslak yönetimi" src="screenshots/08-taslaklar.png" />

🗂️ Kategori Yönetimi
Kullanıcılar kendi kategorilerini oluşturabilir, kategori rengi belirleyebilir ve gelen mesajlarını kategorilerle ilişkilendirebilir.
- Kategori oluşturma ve silme
- Kategorilere renk atama
- Mesajları kategorilere ekleme
- Kategoriye ait mesajları listeleme
<img width="1200" alt="Nexora kategoriler" src="screenshots/09-kategoriler.png" />

🔎 Arama, Filtreleme ve Sayfalama
Gelen Kutusu'nda mesajları bulmayı kolaylaştıran arama ve filtreleme seçenekleri bulunmaktadır.
Filtreleme seçenekleri:
- Gönderen adı ve mesaj konusu
- Okundu / okunmadı durumu
- Yıldızlı mesajlar
- Kategori
- Başlangıç ve bitiş tarihi
- Tarihe göre yeni → eski veya eski → yeni sıralama
Mesajlar 10'arlı sayfalar halinde listelenir. Böylece arama ve filtreleme sonuçları arasında sayfa geçişi yapılabilir.
<img width="1200" alt="Nexora mesaj arama ve filtreleme" src="screenshots/10-arama-filtreleme.png" />

👤 Profil ve Hesap Yönetimi
Kullanıcılar kişisel profil ekranında hesap bilgilerini görüntüleyebilir, ad ve soyadlarını güncelleyebilir ve mevcut parolalarını kullanarak yeni parola belirleyebilir.
Şifre değiştirme işlemi, ASP.NET Core Identity'nin hazır parola doğrulama ve güncelleme mekanizması üzerinden gerçekleştirilir.
<img width="1200" alt="Nexora kullanıcı profili" src="screenshots/11-profil.png" />

📊 Admin Paneli ve Dashboard
Nexora'da Admin rolündeki kullanıcılar için ayrı bir yönetim alanı bulunmaktadır. Yönetici, Dashboard üzerinden sistemin genel durumunu görüntüleyebilir.
Dashboard'da yer alan bilgiler:
- Toplam ve aktif kullanıcı sayısı
- Toplam ve bugün gönderilen mesaj sayısı
- Okunmamış mesaj sayısı
- Çöp kutusuna taşınan mesaj sayısı
- En çok mesaj gönderen kullanıcılar
- En çok kullanılan kategoriler
<img width="1200" alt="Nexora admin dashboard" src="screenshots/12-admin-dashboard.png" />

👥 Kullanıcı ve Rol Yönetimi
Admin panelinde sisteme kayıtlı kullanıcılar listelenebilir, kullanıcı detayları görüntülenebilir, User / Admin rolleri yönetilebilir ve kullanıcı hesapları aktif veya pasif duruma getirilebilir.
Yetkilendirme kontrolleri sayesinde kullanıcı yönetimi sayfalarına yalnızca Admin rolündeki hesaplar erişebilir.
<img width="1200" alt="Nexora kullanıcı ve rol yönetimi" src="screenshots/13-kullanici-yonetimi.png" />

🚩 Mesaj Şikâyet Yönetimi
Kullanıcılar kendilerine gelen bir mesajı belirli bir gerekçeyle şikâyet edebilir. Aynı kullanıcının aynı mesaj için tekrar şikâyet oluşturması engellenir.
Admin panelinde şikâyetler bekleyen / incelenen durumlarına göre görüntülenebilir. Yönetici, şikâyeti ve ilgili mesajı inceleyerek işlemi İncelendi olarak işaretleyebilir.
<img width="1200" alt="Nexora mesaj şikâyet yönetimi" src="screenshots/14-sikayet-yonetimi.png" />

🛠️ Kullanılan Teknolojiler
Teknoloji	Kullanım Amacı
ASP.NET Core MVC	Web uygulaması ve MVC mimarisi
C#	Backend geliştirme
ASP.NET Core Identity	Kimlik doğrulama, kullanıcı ve rol yönetimi
Entity Framework Core	Veritabanı işlemleri ve ilişkiler
Microsoft SQL Server	İlişkisel veritabanı
EF Core Migrations	Veritabanı şemasını oluşturma ve güncelleme
Razor Views	Dinamik kullanıcı arayüzü
HTML & CSS	Sayfa yapısı ve Nexora arayüz tasarımı
JavaScript	Arayüz etkileşimleri


🧱 Proje Yapısı
Nexora, ASP.NET Core MVC yapısı üzerinde düzenlenmiştir.
IdentityMail.Web/
├── Areas/
│   └── Admin/
│       ├── Controllers/       # Dashboard, Users, Reports
│       ├── ViewModels/        # Admin istatistik modelleri
│       └── Views/             # Yönetim paneli ekranları
├── Context/                   # AppDbContext
├── Controllers/               # Auth, Message, Profile, Category
├── DTOs/                      # Form ve veri aktarım modelleri
├── Entities/                  # AppUser, UserMessage, DraftMessage vb.
├── Views/                     # Kullanıcı ekranları ve ortak layout'lar
├── wwwroot/                   # CSS, JavaScript ve statik dosyalar
└── Program.cs                 # Servisler, Identity ve uygulama ayarları
⚙️ Kurulum ve Çalıştırma
Gereksinimler: Projeyle uyumlu .NET SDK, Visual Studio veya benzeri bir IDE ve Microsoft SQL Server.
1. Projeyi bilgisayarınıza indirin veya GitHub üzerinden klonlayın.
2. appsettings.json veya yerel geliştirme ayarlarında DefaultConnection bağlantı bilgisini kendi SQL Server ortamınıza göre düzenleyin.
3. NuGet paketlerini geri yükleyin.
4. Entity Framework Core migration'larını veritabanına uygulayın.
5. Uygulamayı Visual Studio üzerinden başlatın veya proje dizininde dotnet run komutunu çalıştırın.
Bağlantı ayarı örneği:
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=(localdb)\\MSSQLLocalDB;Database=NexoraDb;Trusted_Connection=True;TrustServerCertificate=True;"
  }
}
Visual Studio Package Manager Console üzerinden veritabanını güncellemek için:
Update-Database
Önemli: Admin yetkisi, geliştirme ortamındaki kullanıcıya rol atama ayarlarına bağlıdır. Projede herkese açık varsayılan bir admin parolası bulunmaz. Paylaşmadan önce Program.cs içindeki geliştirme amaçlı sabit admin e-posta atamasını gözden geçirin. Gerçek bağlantı bilgilerini, parolaları ve diğer gizli bilgileri GitHub'a yüklemeyin.

👨‍💻 Geliştirici
Berkant Gülel
Yazılım Mühendisi


<div align="center">

✉️ Nexora
Your Messages. Your Space.
Developed by Berkant Gülel © 2026
</div>
