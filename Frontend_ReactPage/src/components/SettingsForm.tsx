import React, { useState } from 'react';
import { SystemSettings as SystemSettingsDTO } from '../types/settingsTypes';

interface Props {
  initialSettings: SystemSettingsDTO;
  onSave: (settings: SystemSettingsDTO) => void;
}

const SettingsForm = ({ initialSettings, onSave }: Props) => {
  const [settings, setSettings] = useState<SystemSettingsDTO>({
    ...initialSettings,
    language: initialSettings.language ?? 'de',
    fontSize: initialSettings.fontSize ?? 'normal',
    autoSendReports: initialSettings.autoSendReports ?? false,
    downloadOnly: initialSettings.downloadOnly ?? false,
    tokenConfigs: initialSettings.tokenConfigs ?? [],
    backgroundImageUrl: initialSettings.backgroundImageUrl ?? '',
  });

  const handleInputChange = <K extends keyof SystemSettingsDTO>(
    key: K,
    value: SystemSettingsDTO[K]
  ) => {
    setSettings((prev) => ({
      ...prev,
      [key]: value,
    }));
  };

  const handleSubmit = (e: React.FormEvent) => {
    e.preventDefault();
    onSave(settings);
  };

  return (
    <form onSubmit={handleSubmit} className="space-y-6">
      {/* Sprache */}
      <div>
        <label className="block font-medium">Sprache:</label>
        <select
          value={settings.language}
          onChange={(e) => handleInputChange('language', e.target.value)}
          className="border p-2 rounded w-full"
        >
          <option value="de">Deutsch</option>
          <option value="en">Englisch</option>
        </select>
      </div>

      {/* Schriftgröße */}
      <div>
        <label className="block font-medium">Schriftgröße:</label>
        <select
          value={settings.fontSize}
          onChange={(e) => handleInputChange('fontSize', e.target.value as any)}
          className="border p-2 rounded w-full"
        >
          <option value="small">Klein</option>
          <option value="normal">Normal</option>
          <option value="large">Groß</option>
        </select>
      </div>

      {/* Automatischer E-Mail-Versand */}
      <div>
        <label className="block font-medium">
          <input
            type="checkbox"
            checked={settings.autoSendReports ?? false}
            onChange={(e) => handleInputChange('autoSendReports', e.target.checked)}
            className="mr-2"
          />
          Berichte automatisch per E-Mail versenden
        </label>
      </div>

      {/* Nur Download */}
      <div>
        <label className="block font-medium">
          <input
            type="checkbox"
            checked={settings.downloadOnly ?? false}
            onChange={(e) => handleInputChange('downloadOnly', e.target.checked)}
            className="mr-2"
          />
          Berichte nur zum Download bereitstellen
        </label>
      </div>

      {/* Hintergrundbild (nur Anzeige – kein Upload) */}
      <div>
        <label className="block font-medium mb-1">Hintergrundbild (nur Anzeige):</label>
        {settings.backgroundImageUrl ? (
          <img src={settings.backgroundImageUrl} alt="Hintergrundbild" className="max-w-sm rounded" />
        ) : (
          <p className="text-gray-500 italic">Kein Bild festgelegt</p>
        )}
      </div>

      <button
        type="submit"
        className="bg-blue-600 hover:bg-blue-700 text-white px-6 py-2 rounded"
      >
        Einstellungen speichern
      </button>
    </form>
  );
};

export default SettingsForm;
