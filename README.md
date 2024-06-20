# Boren.MiniAPI.WebApplication
## ASP.NET Core Minimal API 基本架構
將API routing, mapping, 程式邏輯 全寫在Program.cs 的Main 當中。也可以設定、產生OpenAPI 文件
## 如何優化
隨著提供的API 數量增加，Program.cs 就會變得越來越難維護。此時可以將功能模組化、歸類。並配合依賴注入，除了讓程式架構更乾淨以外，也能讓程式物件導向化。
## 加入身分驗證
待研究...