import { defineConfig } from 'vite'

// 文件入口，input file
const entryFileNames = {
    bQuery: './src/bQuery.ts',
}

export default defineConfig(({ command, mode }) => {
    var isProd = mode === 'production';
    const isWatch = !isProd;
    console.log(process.env.buildWatch, isWatch)
    return {
        //plugins: [
        //    legacy({
        //        targets: ['defaults']
        //    })
        //],
        base: "./",
        // mode: mode, // 'development' 用于开发，'production' 用于构建
        build: {
            outDir: './',
            // assetsDir: 'assets',
            sourcemap: !isProd,
            chunkSizeWarningLimit: 20, // 20 KB
            watch: isWatch,
            minify: isProd,
            lib: {
                fileName: "[name]",
                formats: ["es"],
            },
            rollupOptions: {
                input: entryFileNames,
                // 用于排除不需要打包的依赖
                // external: ["react", "react-dom"],
                output: {
                    //  chunk 包的文件名，默认 [name]-[hash].js
                    chunkFileNames: '[name].js',
                    // 定义 chunk 包的名 和规则
                    manualChunks: null,
                    // 资源文件打包变量名， 默认值："assets/[name]-[hash][extname]"
                    assetFileNames: (fileInfo) => {
                        console.log("assetFileNames", fileInfo);
                        if (fileInfo.name == 'style.css'){
                            return 'index.css';
                        }
                        const fileName = fileInfo.name;
                        // js 是entry，所以这里不会生效
                        if (fileName.endsWith('.js') || fileName.endsWith('.ts')) {
                            return `[name].min.[ext]`
                        }

                        if (fileName.endsWith('.css')) {
                            return `[name].min.[ext]`
                        }

                        if (fileName.endsWith('.svg')
                            || fileName.endsWith('.png')
                            || fileName.endsWith('.jpg')
                            || fileName.endsWith('.jpeg')
                        ) {
                            return `image/[name].[ext]`
                        }

                        if (fileName.endsWith('.ttf')
                            || fileName.endsWith('.otf')
                            || fileName.endsWith('.eot')
                            || fileName.endsWith('.woff')
                            || fileName.endsWith('.woff2')
                        ) {
                            return `font/[name].[ext]`
                        }
                        return `[ext]/[name].[ext]`
                    },
                    // 入口文件 input 配置所指向的文件包名 默认值："[name].js" 
                    entryFileNames: (fileInfo) => {
                        console.log("entryFileNames", fileInfo.facadeModuleId);
                        return '[name].min.js';
                    },
                },
            },
        },
    }
}
)