export default {
  content: [
    "./index.html",
    "./src/**/*.{vue,js,ts,jsx,tsx}",
  ],
  darkMode: 'class',
  theme: {
    screens: {
      sm: '545px',
      md: '745px',
      lg: '950px',
      xl: '1440px',
    },
    extend: {
      colors: {
        bgColorPrimary: '#ffffff',
        bgColorSecondary: '#ffffff',
        bgColorPrimaryDark: '#21252b',
        bgColorSecondaryDark: '#2c313c',
        borderColor: '#eaeaea',
        borderColorDark: '#404759',
        inputBorderColor: '#b0b0b0',


        shadowColor: '#000000',
        shadowColorDark: '#404759',
        dropDownHoverColor: '#dd524c',


        textPrimary: '#222222',
        textSecondary: '#717171',
        textPrimaryDark: '#cdcdcd',
        textSecondaryDark: '#818181',
        modalTextSecondary: '#797979',


        logoSelected: '#000000',
        logoSecondary: '#717171',
        logoAccent: '#dddddd',
        logoPrimary: '#ff385c',
      }
    },
  },
  plugins: [],
}