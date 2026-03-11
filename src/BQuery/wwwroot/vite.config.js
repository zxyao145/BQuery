import { defineConfig } from 'vite'

export default defineConfig(({ mode }) => {
  const isProd = mode === 'production';

  return {
    base: './',
    build: {
      outDir: 'dist',
      emptyOutDir: true,
      sourcemap: !isProd,
      chunkSizeWarningLimit: 20,
      watch: isProd ? null : {},
      minify: isProd,
      lib: {
        entry: './src/index.ts',
        name: 'bQuery',
        formats: ['es', 'umd'],
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
