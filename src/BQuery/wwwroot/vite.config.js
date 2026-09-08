import { defineConfig } from 'vite'

export default defineConfig(({ mode }) => {
  const isProd = mode === 'production';

  return {
    base: './',
    build: {
      outDir: 'dist',
      target: ['chrome107', 'edge107', 'firefox104', 'safari16'],
      emptyOutDir: true,
      sourcemap: !isProd,
      chunkSizeWarningLimit: 20,
      watch: isProd ? null : {},
      minify: isProd,
      lib: {
        entry: './src/index.ts',
        name: 'bQuery',
        formats: ['umd'],
        fileName: (format) => {
          if(format === "es") {
            return "bQuery.min.mjs";
          }
          return "bQuery.min.js";
        },
      },
    },
  };
});
