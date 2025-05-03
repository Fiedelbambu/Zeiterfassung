// tailwind.config.js oder tailwind.config.ts

export default {
  content: ["./src/**/*.{js,ts,jsx,tsx}"],
  theme: {
    extend: {
      fontSize: {
        small: "12px",
        normal: "16px",
        large: "20px",
      },
    },
  },
  plugins: [],
};
