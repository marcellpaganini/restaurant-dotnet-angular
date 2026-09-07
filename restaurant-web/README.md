# Restaurant Web (Angular 14)

Angular client for the Restaurant API at `C:\src\restaurant`.

## Prerequisites

- Node.js 14–16 recommended for Angular 14 (Node 18+ may warn)
- Restaurant API running on `http://localhost:5292`
- SQL script applied: `C:\src\restaurant\scripts\CreateTables.sql`

## Setup

```bash
cd C:\src\restaurant-web
npm install
npm start
```

Open `http://localhost:4200`.

The dev server proxies `/api/*` to `http://localhost:5292` via `proxy.conf.json`.

## Pages

- `/meals` – GET `/api/meals`
- `/clients` – GET `/api/clients`
- `/orders` – GET `/api/orders`
