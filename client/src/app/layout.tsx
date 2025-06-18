import { roboto } from '@/types/theme';
import type { Metadata } from "next";
import ThemeRegistry from '@/components/ThemeRegistry/ThemeRegistry';
import "./globals.css";

export const metadata: Metadata = {
  title: "Project V",
  description: "Project created to upgrade development skills",
};

export default function RootLayout({
  children,
}: Readonly<{
  children: React.ReactNode;
}>) {
  return (
    <html lang="en" className={roboto.variable}>
      <body>
        <ThemeRegistry>
          {children}
        </ThemeRegistry>
      </body>
    </html>
  );
}
