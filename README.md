# 🛒 E-Shop Klient – Inlämningsuppgift Webbutveckling

Detta projekt är en JavaScript-baserad frontend för en E-handelstjänst, utvecklad som en inlämningsuppgift i kursen **Webbutveckling**. Applikationen kommunicerar med en backend som tidigare utvecklats i **C# fortsättningskursen**.

## 📋 Funktionalitet

Frontend-applikationen erbjuder följande funktioner:

- Visa lista med **leverantörer**
- Visa lista med **produkter**
- Visa lista med **kunder**
- **Lägg till nya produkter** via ett formulär (utan bild) funktionen hittas längst ner på produkt sidan via ett knapptryck "Add New Product".
- Navigera mellan sidor via en **responsiv meny**

## 🧱 Teknisk struktur

- Byggd med **ren JavaScript**, **HTML** och **CSS**
- **Modulär kodstruktur**: Funktionalitet är uppdelad i separata JS-moduler med `export/import`
- **Responsiv design**: Anpassar sig till olika skärmstorlekar
- Fokus på **DRY-principen**: Kodduplicering undviks så mycket som möjligt
- Enkel och tydlig **navigationsmeny** för att växla mellan vyerna

## 🛠 Installation

1. Klona detta repo:
   ```bash
   git clone https://github.com/thelundix/E-SHOP-CLIENT.git
Öppna projektet i din kodeditor

Kör backend-applikationen (från C#-kursen)

Starta index.html i din webbläsare (du kan använda en liveserver som t.ex. Live Server i VS Code)

OBS! Backend ska vara igång för att applikationen ska kunna hämta och spara data.
