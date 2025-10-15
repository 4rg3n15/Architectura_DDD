const BASE = (
  import.meta.env.VITE_API_BASE_URL || "http://localhost:5071/api"
).replace(/\/$/, "");

async function request(path, { method = "GET", body, headers } = {}) {
  const opts = {
    method,
    headers: { "Content-Type": "application/json", ...(headers || {}) },
  };
  if (body !== undefined) opts.body = JSON.stringify(body);

  const res = await fetch(`${BASE}${path}`, opts);
  if (!res.ok) {
    let msg = await res.text().catch(() => "");
    try {
      const j = JSON.parse(msg);
      msg = j?.message || j?.error || msg;
    } catch {}
    throw new Error(msg || `HTTP ${res.status}`);
  }
  const ct = res.headers.get("content-type") || "";
  return ct.includes("application/json") ? res.json() : res.text();
}

export const http = {
  get: (p) => request(p),
  post: (p, b) => request(p, { method: "POST", body: b }),
  put: (p, b) => request(p, { method: "PUT", body: b }),
};
