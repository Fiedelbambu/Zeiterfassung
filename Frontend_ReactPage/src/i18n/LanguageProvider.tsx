import React, { useState } from 'react';
import { LanguageContext } from './LanguageContext';

export default function LanguageProvider({ children }: { children: React.ReactNode }) {
  const [language, setLanguage] = useState('de');

  return (
    <LanguageContext.Provider value={{ language, setLanguage }}>
      {children}
    </LanguageContext.Provider>
  );
}
