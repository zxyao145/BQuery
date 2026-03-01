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
        entry: './src/bQuery.ts',
        name: 'bQuery',
        formats: ['iife'],
        fileName: () => 'bQuery.min.js',
      },
    },
  };
});
