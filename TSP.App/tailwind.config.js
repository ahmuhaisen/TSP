/** @type {import('tailwindcss').Config} */
module.exports = {
  content: [
    "./src/**/*.{html,ts}",
  ],
  theme: {
    extend: {
      fontFamily: {
        sans: ['Work Sans', 'sans-serif'],
      },
      colors: {
        'primary-dark': '#1d4f91',
        'primary-dark-2': '#0368b8',
        'primary-light': '#009cd5',
        'secondary': '#b8bb34',
        'tertiary': '#72d8f7',
      },
    },
  },
  plugins: [
    require('tailwindcss-textshadow'),
  ],
}