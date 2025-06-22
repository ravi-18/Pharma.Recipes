
# 💊 Pharma Recipes API

Sistem digital dokumentasi resep produksi obat — dibuat sebagai bagian dari technical test untuk posisi **Back-End Developer**.

---

## 📦 Fitur Utama

- ✅ Manajemen Resep Obat (resep, langkah, parameter)
- ✅ Struktur langkah bersifat hierarkis (support sub-step tak terbatas)
- ✅ Manajemen pengguna (register, login, ganti password)
- ✅ JWT Authentication & Role-based Authorization
- ✅ Swagger UI untuk dokumentasi API
- ✅ PostgreSQL sebagai database utama

---

## 🔧 Setup & Instalasi

### 1. Clone Repository

```bash
git clone https://github.com/yourusername/pharma-recipes-api.git
cd pharma-recipes-api
```

### 2. Konfigurasi Database (PostgreSQL)

Tambahkan connection string di `appsettings.Development.json` atau `secrets.json`:

```json
"ConnectionStrings": {
  "DefaultConnection": "Host=localhost;Port=5432;Database=pharma_db;Username=postgres;Password=yourpassword"
}
```

### 3. Konfigurasi JWT

```json
"Jwt": {
  "Key": "pharma_super_secret_key_2025_very_secure!",
  "Issuer": "PharmaRecipes"
}
```

### 4. Jalankan Migration & Update DB

```bash
dotnet ef database update
```

---

## ▶️ Cara Menjalankan

Untuk menjalankan aplikasi, pastikan Anda sudah menginstall .NET SDK 8.0 dan PostgreSQL.
Jika Menggunakan .NET CLI:
```bash
dotnet run
```

Jika Menggunakan Docker Compose:
```bash
docker-compose up --build
```

Jika Tanpa Docker Compose, Build image:
```bash
docker build -t pharma-api ./Pharma.Recipes.API/
```

Perlu Jalankan container dan hubungkan ke PostgreSQL:
```bash
docker run -d -p 5000:80 \
  -e "ConnectionStrings__DefaultConnection=Host=host.docker.internal;Port=5432;Database=pharma_db;Username=postgres;Password=postgres" \
  -e "Jwt__Key=your_super_secret_key_here_32_chars_minimum" \
  -e "Jwt__Issuer=Pharma" \
  --name pharma-api pharma-api
```
Pastikan PostgreSQL sudah berjalan (bisa lokal atau container lain)


Akses via browser:

```
https://localhost:{port}/swagger
```

---

## 🔐 Autentikasi & Autorisasi

### Endpoint Auth

| Endpoint                          | Method | Deskripsi                  |
|-----------------------------------|--------|----------------------------|
| `/api/auth/register`             | POST   | Registrasi pengguna baru   |
| `/api/auth/login`                | POST   | Login & dapatkan token JWT |
| `/api/auth/change-password`      | POST   | Ganti password             |

> Setelah login, gunakan token untuk akses endpoint lain:
```
Authorization: {your-token}
```

---

## 🧪 Testing API via Swagger

- Buka: `https://localhost:{port}/swagger`
- Klik 🔐 **Authorize**
- Masukkan: `{token}`
- Uji semua endpoint dengan token yang valid

---

## 📚 Dokumentasi Endpoint

### 📄 Recipe

| Endpoint             | Method | Auth  | Deskripsi               |
|----------------------|--------|-------|-------------------------|
| `/api/recipe`       | GET    | ✅    | Ambil semua resep       |
| `/api/recipe`       | POST   | ✅    | Tambah resep baru       |
| `/api/recipe/{id}`  | GET    | ✅    | Ambil detail resep      |
| `/api/recipe/{id}`  | PUT    | ✅    | Ubah resep              |
| `/api/recipe/{id}`  | DELETE | ✅    | Hapus resep             |

### 🧩 Step

| Endpoint                            | Method | Auth  | Deskripsi                         |
|-------------------------------------|--------|-------|-----------------------------------|
| `/api/recipe/{recipeId}/steps`      | GET    | ✅    | Ambil semua step untuk resep      |
| `/api/recipe/{recipeId}/steps`      | POST   | ✅    | Tambah step untuk resep           |
| `/api/recipe/{recipeId}/steps/{id}` | GET    | ✅    | Ambil step detail (tree structure)|
| `/api/recipe/{recipeId}/steps/{id}` | PUT    | ✅    | Ubah step untuk resep             |
| `/api/recipe/{recipeId}/steps/{id}` | DELETE | ✅    | Hapus step beserta substeps       |

---

## 🛠 Struktur Folder

```
Pharma.Recipes/
├── Controllers/
├── Data/
├── Dtos/
├── Enums/
├── Mapper/
├── Migrations/
├── Models/
├── Repositories/
├── appsettings.json
├── Dockerfile
├── Program.cs
```

---

## 🧬 Database Schema

```
AppUser
-------
Id (GUID, PK)
Username (string)
Email (string)
PasswordHash (string)
Role (string)
CreatedAt, CreatedBy, ModifiedAt, ModifiedBy 

Recipe
------
Id (GUID, PK)
Name (string)
Description (string)
CreatedAt, CreatedBy, ModifiedAt, ModifiedBy

Step
----
Id (GUID, PK)
RecipeId (FK)
ParentStepId (nullable FK)
Title (string)
Sequence (int)
CreatedAt, CreatedBy, ModifiedAt, ModifiedBy

StepParameter
-------------
Id (GUID, PK)
StepId (FK)
Name (enum/string)
DataType (string)
Value (string)
Description (nullable)
CreatedAt, CreatedBy, ModifiedAt, ModifiedBy
```

Relasi:
- `Recipe 1 — * Step`
- `Step 1 — * Step` (self-reference)
- `Step 1 — * StepParameter`

---

## 📌 Teknologi Digunakan

- .NET 8 Web API
- Entity Framework Core
- PostgreSQL
- AutoMapper
- JWT Auth (Microsoft.IdentityModel)
- BCrypt.Net (hashing password)
- Swagger / Swashbuckle

---

## ✅ Tips Tambahan

- Gunakan `User.Identity.Name` untuk ambil username dari token
- Proteksi endpoint dengan `[Authorize]` dan `[Authorize(Roles = "Admin")]`
- Gunakan DTO untuk response agar menghindari nested/cyclic JSON

---

## 🙌 Author

Developed by: **Ravi Algifari**  
Disiapkan untuk keperluan **technical test Back-End Developer**.
