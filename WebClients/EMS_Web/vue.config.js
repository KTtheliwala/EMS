const webpack = require('webpack');
const path = require('path');
const crypto = require('crypto');
const emotionalNonce = crypto.randomBytes(16).toString('base64');
const emotionalNonce1 = crypto.randomBytes(16).toString('base64');
const MiniCssExtractPlugin = require('mini-css-extract-plugin');
const CspHtmlWebpackPlugin = require('csp-html-webpack-plugin');
module.exports = {
  lintOnSave: false,
  publicPath: process.env.VUE_APP_BASE_URL,
  productionSourceMap: false,
  configureWebpack: {
    devServer: {
      hot: false
    },
    devtool: process.env.VUE_APP_ENVIRONMENT === "production" ? false : 'source-map',
    output: {
      filename: process.env.VUE_APP_ENVIRONMENT === "production" ? 'js/[contenthash:8].min.js' : 'js/[name].js',
      chunkFilename: process.env.VUE_APP_ENVIRONMENT === "production" ? 'js/[contenthash:8].min.js' : 'js/[name].js'
    },
    module: {
      rules: [
        {
          test: /\.s(c|a)ss$/,
          use: [
            MiniCssExtractPlugin.loader,
            {
              loader: 'css-loader',
              options: {
                sourceMap: false,
                import: true
              }
            },
            {
              loader: 'sass-loader',
              options: {
                implementation: require('sass'),
                sassOptions: {
                  data: `@import "@/assets/css/themeprimevue.scss";`,
                  includePaths: [
                    'assets',
                  ],
                  outputStyle: "compressed",
                  sourceMap: false
                }
              }
            }
          ]
        }
      ]
    },
    plugins: [
      new MiniCssExtractPlugin({
        filename: '[hash:8].css',
        chunkFilename: "[hash:8].css",
        attributes: {
          nonce: emotionalNonce,
        }
      }),
      new CspHtmlWebpackPlugin({
        'font-src': ["'self'", 'fonts.gstatic.com'],
        'frame-src': "'self'",
        'img-src': ["'self'", "data:", "blob:"],
        'script-src': ["'self'", "'strict-dynamic'", `'nonce-${emotionalNonce1}'`],
        'style-src': ["'self'", "fonts.googleapis.com", `'nonce-${emotionalNonce}'`, `'nonce-56b+97z7RhNTcMh4VKGXJg=='`]
      }, {
        enabled: false,
        hashingMethod: 'sha256',
        hashEnabled: {
          'script-src': false,
          'style-src': false
        },
        nonceEnabled: {
          'script-src': true,
          'style-src': true
        },
      })
    ],
  },
};