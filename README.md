# PayrollManagement.API

## 🧩 Proje Hakkında
Çalışanların maaş, günlük ücret ve fazla mesai hesaplamalarını yöneten .NET 8 tabanlı bir API uygulamasıdır.

## ⚙️ Teknolojiler
- .NET 8 Web API  
- Entity Framework Core  
- MSSQL (TSQL, Stored Procedures, UDF)
- Swagger  
- JWT Authentication  /Örnek CORS Konfigürasyonu 
- Katmanlı Mimari (API - Service - Data - Core)
- XUnit ve WebApplicationFactory


## 🚀 Kurulum
1. Repository'i klonlayın:
   ```bash
   git clone https://github.com/kullaniciadi/PayrollManagement.git
   cd PayrollManagement

## 🚀 Erişim

1. Test senaryoları otomatik çalışacak şekilde ayarlanmıştır.
2. dotnet run ile ayağa kaldırılıp akabinde localhost adresine gidilerek swagger üzerinden testler yapılabilir
3. JWT Token almak için /login adresine istek atılır çıkan access_token Sağ üstteki Authorize  butonuna tıklanarak çıkan alana yapıştırılır. Başına 'Bearer' eklemeye gerek yok
   ```bash
     {"email": "admin", "password": "admin123","twoFactorCode": "string", "twoFactorRecoveryCode": "string"}
4 token girdikten sonra testler yapılabilir. 

## Erişim
Herhangi bir önyüz talebi gelmediğinden yapılmamıştır. CORS ayarları yine de test ve dış dünya için ayrı ayrı girilmiştir.


# Proje Adresi  
```bash
https://github.com/mertyllmzz1/PayrollManagament.API
