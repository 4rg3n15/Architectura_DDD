import { defineConfig } from "vite";
import react from "@vitejs/plugin-react";

export default defineConfig({
  plugins: [react()],
  server: {
    port: 5173,
    proxy: {
      // If you prefer same-origin dev calls without CORS, uncomment and adjust:
      // '/api': {
      // target: 'https://localhost:7235',
      // changeOrigin: true,
      // secure: false
      // }
    },
  },
});
