# 🔐 Authentication Project (.NET 8 Web API)

Bu proje, JWT tabanlı kullanıcı kimlik doğrulaması ve yetkilendirmesi sağlayan bir ASP.NET Core Web API projesidir.

## 🚀 Özellikler

- Kullanıcı Kaydı (Register)
- Kullanıcı Girişi (Login)
- JWT Token oluşturma ve doğrulama
- Kullanıcı bilgilerini görüntüleme (`/getperson`)
- Tüm kullanıcıları listeleme (`/allusers`)
- Token doğrulama, geçersizlik, süre kontrolü
- Şifre hashleme (PBKDF2 + HMACSHA512)
- Validation: `[Required]`, `[EmailAddress]`, `[MinLength]`
- `ModelState` ile hata kontrolü

## 🧩 Katmanlar

- `DTOs`: Giriş/Çıkış verileri için veri transfer objeleri
- `Services`: İş mantığı ve token işlemleri
- `Handlers`: Şifreleme mantığı
- `Controllers`: API uç noktaları
- `Contexts`: DbContext yapılandırması (EF Core)
- `Program.cs`: DI, JWT, Auth ayarları

## ⚙️ Teknolojiler

- ASP.NET Core 8.0
- Entity Framework Core
- SQL Server
- JWT (Json Web Token)
- Swagger (API Test Arayüzü)
- Curl / Postman ile test edilebilir

## 🔐 API Uç Noktaları

| Endpoint | Açıklama | Yetki |
|----------|----------|--------|
| `POST /api/Auth/register` | Yeni kullanıcı kaydı | Herkes |
| `POST /api/Auth/login` | Giriş yapar, token döner | Herkes |
| `GET /api/Auth/getperson` | Token içindeki kullanıcıyı döner | Auth gerekir |
| `GET /api/Auth/allusers` | Tüm kullanıcıları listeler | Auth gerekir |

## 🛠 Kurulum

Projeyi klonla veya indir
appsettings.json dosyasına kendi Jwt:Key, Issuer, Audience, ConnectionStrings değerlerini gir
dotnet ef database update komutu ile veritabanını oluştur
Projeyi çalıştır (Ctrl + F5 veya dotnet run)
Swagger üzerinden test edebilirsin

## 🧪 Test

### Curl ile

```bash
curl -X POST "https://localhost:7163/api/Auth/login" ^
 -H "Content-Type: application/json" ^
 -d "{\"email\":\"user1@gmail.com\", \"password\":\"user11234\"}"