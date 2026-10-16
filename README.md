<img width="1672" height="941" alt="Nexora_ Akıllı Mesajlaşma Platformu" src="https://github.com/user-attachments/assets/1900ada8-06a7-46c2-8e03-94907ea4fc19" />

<br>

<div align="center">

# 💜 Nexora

### ASP.NET Core MVC & Identity-Based Messaging Platform

Nexora; kullanıcıların güvenli şekilde mesajlaşabildiği, mesajlarını
kategorilere ayırabildiği, taslaklarını yönetebildiği ve gelişmiş
arama özelliklerini kullanabildiği modern bir mesajlaşma platformudur.
ASP.NET Core Identity tabanlı kimlik doğrulama, rol yönetimi ve
yönetici paneli ile kullanıcı ve mesaj yönetimini bir araya getirir.

<br>

<img src="https://img.shields.io/badge/ASP.NET_Core_MVC-512BD4?style=for-the-badge&logo=dotnet&logoColor=white" />
<img src="https://img.shields.io/badge/C%23-239120?style=for-the-badge&logo=csharp&logoColor=white" />
<img src="https://img.shields.io/badge/SQL_Server-CC2927?style=for-the-badge&logo=microsoftsqlserver&logoColor=white" />
<img src="https://img.shields.io/badge/Entity_Framework_Core-512BD4?style=for-the-badge&logo=dotnet&logoColor=white" />
<img src="https://img.shields.io/badge/ASP.NET_Core_Identity-6D28D9?style=for-the-badge&logo=dotnet&logoColor=white" />

<br><br>

<img src="https://img.shields.io/badge/Bootstrap-7952B3?style=flat-square&logo=bootstrap&logoColor=white" />
<img src="https://img.shields.io/badge/HTML5-E34F26?style=flat-square&logo=html5&logoColor=white" />
<img src="https://img.shields.io/badge/CSS3-1572B6?style=flat-square&logo=css&logoColor=white" />
<img src="https://img.shields.io/badge/JavaScript-F7DF1E?style=flat-square&logo=javascript&logoColor=black" />

</div>

<br>

---

---

## ✨ Proje Hakkında

Nexora, ASP.NET Core MVC ve ASP.NET Core Identity kullanılarak geliştirilen modern bir mesajlaşma ve kullanıcı yönetim platformudur.

Kullanıcılar platform üzerinden kayıt olabilir, giriş yapabilir, gelen kutularını yönetebilir, mesaj gönderebilir, taslak oluşturabilir, mesajlarını yıldızlayabilir, çöp kutusuna taşıyabilir ve kategorilere ayırabilir. Ayrıca profil bilgilerini güncelleyebilir ve şifre işlemlerini güvenli şekilde gerçekleştirebilir.

Yönetim tarafında ise admin paneli üzerinden kullanıcılar görüntülenebilir, roller yönetilebilir, hesap durumları güncellenebilir ve kullanıcılar tarafından oluşturulan mesaj şikâyetleri takip edilebilir. Böylece hem kullanıcı paneli hem de yönetim paneli tek proje içinde bir araya getirilmiştir.

---

---

## 🔐 Giriş ve Kayıt Ekranları

Nexora, ASP.NET Core Identity altyapısı ile kullanıcı kayıt ve giriş işlemlerini yönetmektedir. Kullanıcılar kendilerine ait hesaplar oluşturarak mesajlaşma platformuna güvenli bir şekilde erişebilir.

### 🔑 Kullanıcı Girişi

Kayıtlı kullanıcılar e-posta adresleri ve şifreleri ile sisteme giriş yapabilir. Rol tabanlı yetkilendirme sayesinde kullanıcılar kendi mesajlaşma paneline, yöneticiler ise Admin Dashboard ekranına yönlendirilir.

<img width="1919" height="1079" alt="Ekran görüntüsü 2026-10-16 214628" src="https://github.com/user-attachments/assets/6ae32446-29b8-457e-8696-990f811a8a01" />

<br>

### 📝 Kullanıcı Kaydı

Yeni kullanıcılar ad, soyad, kullanıcı adı, e-posta adresi ve şifre bilgileriyle hesap oluşturabilir. ASP.NET Core Identity sayesinde şifre güvenliği, kullanıcı doğrulamaları ve benzersiz e-posta kontrolü sağlanmaktadır.

<img width="1919" height="1079" alt="Ekran görüntüsü 2026-10-16 214635" src="https://github.com/user-attachments/assets/8fa60245-982c-4572-aedf-d6bb3260b747" />

---

---

## 📥 Gelen Kutusu ve Mesaj Yönetimi

Nexora'nın gelen kutusu, kullanıcıların kendilerine gönderilen mesajları tek bir ekran üzerinden görüntüleyip yönetebildiği modern bir arayüze sahiptir.

Kullanıcılar; gönderen adı ve mesaj konusuna göre arama yapabilir, okundu/okunmadı durumuna, kategoriye ve tarih aralığına göre filtreleme uygulayabilir. Mesajlar tarihe göre sıralanabilir ve sayfalama ile listelenebilir.

Ayrıca mesajları yıldızlama, kategorilere ayırma, çöp kutusuna taşıma ve yanıtlama işlemleri gerçekleştirilebilir. Sidebar üzerinde okunmamış mesaj sayısı görüntülenir.

### 🌙 Koyu Tema

<img width="1919" height="1079" alt="4" src="https://github.com/user-attachments/assets/fda9c349-834d-4538-8b38-44afe51bbd1b" />

<br>

### ☀️ Açık Tema

<img width="1916" height="1079" alt="3" src="https://github.com/user-attachments/assets/cfced11a-a001-4972-8c58-304ba4591ea4" />

---
<img width="1919" height="1079" alt="8" src="https://github.com/user-attachments/assets/32cd0a7b-6a4d-47bb-a50a-722b5723cb2b" />

---

## 📩 Gelen Mesaj Detayı

Nexora’da kullanıcılar gelen kutusunda yer alan herhangi bir mesaja tıklayarak mesajın detay sayfasına ulaşabilmektedir.

Bu detay ekranında gönderen kullanıcı bilgileri, gönderim tarihi, mesajın konusu ve tam içerik yapısı sade ve okunabilir bir arayüz ile sunulmaktadır.

Ayrıca kullanıcı bu ekran üzerinden ilgili mesaja doğrudan cevap verebilir ve tekrar gelen kutusuna dönebilir.

Mesaj okundu durumunda sistem tarafından okunma bilgisi de kullanıcıya gösterilmektedir.

<img width="1919" height="1079" alt="6" src="https://github.com/user-attachments/assets/e8d79a79-b083-4e1c-a1fc-de7d8393e9ed" />

<br>

<img width="1919" height="1079" alt="5" src="https://github.com/user-attachments/assets/25b4f20a-a645-4a3f-9092-2affacfff312" />

---

---

## ✉️ Yeni Mesaj Oluşturma ve Yanıtlama

Nexora, kullanıcıların sistemde kayıtlı diğer kullanıcılara mesaj gönderebilmesini sağlayan sade ve modern bir mesaj oluşturma arayüzüne sahiptir.

Kullanıcılar alıcının e-posta adresini, mesaj konusunu ve içeriğini belirleyerek yeni mesaj oluşturabilir. Mesaj gönderimi sırasında alıcının sistemde kayıtlı olup olmadığı kontrol edilir.

Mesaj oluşturma ekranında;

- Kayıtlı kullanıcılara mesaj gönderme
- Alıcı, konu ve mesaj içeriği belirleme
- Mesajı göndermeden taslak olarak kaydetme
- Gelen mesajları doğrudan yanıtlama

işlemleri gerçekleştirilebilmektedir.

Yanıtlama işleminde alıcının e-posta adresi otomatik olarak doldurulur ve mesaj konusuna **Re:** ifadesi eklenir. Böylece kullanıcı mevcut mesaja kolayca cevap verebilir.

### ☀️ Açık Tema

<img width="1919" height="1079" alt="8" src="https://github.com/user-attachments/assets/6563f6f4-9ea6-437e-9f1f-30459c8461c8" />

<br>

### 🌙 Koyu Tema

<img width="1919" height="1079" alt="7" src="https://github.com/user-attachments/assets/b9deaa73-8a94-4f85-95b2-f2024029e939" />

---

---

## 🔎 Gelişmiş Arama ve Filtreleme

Nexora, kullanıcıların mesajlarına hızlı ve kolay şekilde ulaşabilmesi için gelişmiş arama, filtreleme ve sıralama özellikleri sunmaktadır.

Kullanıcılar gelen kutularındaki mesajları gönderen adı veya mesaj konusuna göre arayabilir. Okunma durumu, kategori ve tarih aralığına göre filtreleme yapabilir, yalnızca yıldızlı mesajları listeleyebilir ve mesajları gönderim tarihine göre sıralayabilir.

Birden fazla filtre aynı anda uygulanabilir. Ayrıca sayfalama desteği sayesinde çok sayıda mesaj düzenli şekilde görüntülenebilir.

<img width="1919" height="1079" alt="5" src="https://github.com/user-attachments/assets/9be97d85-9cba-4937-97f0-632601ffbce0" />

---

---

## 📤 Gönderilen Mesajlar

Nexora, kullanıcıların gönderdikleri mesajları tek bir ekran üzerinden görüntüleyebilmelerini ve takip edebilmelerini sağlar.

Gönderilenler bölümünde alıcının adı, e-posta adresi, mesaj konusu, içerik önizlemesi ve gönderim tarihi listelenmektedir.

Kullanıcılar gönderdikleri mesajların detaylarını inceleyebilir, alıcı tarafından okunup okunmadığını takip edebilir ve ihtiyaç duyduklarında mesajları çöp kutusuna taşıyabilir.

Mesajlar en yeniden eskiye doğru sıralanarak kullanıcıya düzenli bir görünüm sunulur.

<img width="1919" height="1079" alt="6" src="https://github.com/user-attachments/assets/e427dfd8-ad2b-4267-9691-6da8168348f7" />

---

