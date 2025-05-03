import React, { useState } from 'react';
import { SystemSettings, TokenConfig, LoginMethod, FontSizeOption } from '../types/settingsTypes';
import { useTranslation } from 'react-i18next';

interface Props {
  initialSettings: SystemSettings;
  onSave: (settings: SystemSettings) => void;
}

const SettingsForm = ({ initialSettings, onSave }: Props) => {
  const { t, i18n } = useTranslation();
  const [settings, setSettings] = useState<SystemSettings>(initialSettings);

  const handleChange = <K extends keyof SystemSettings>(key: K, value: SystemSettings[K]) => {
    setSettings((prev) => ({ ...prev, [key]: value }));
  };

  const handleTokenConfigChange = (index: number, key: keyof TokenConfig, value: any) => {
    const updated = [...settings.tokenConfigs];
    updated[index] = { ...updated[index], [key]: value };
    setSettings((prev) => ({ ...prev, tokenConfigs: updated }));
  };

  const addTokenConfig = () => {
    setSettings((prev) => ({
      ...prev,
      tokenConfigs: [...prev.tokenConfigs, { loginType: 'Passwort', validityDuration: '01:00:00' }]
    }));
  };

  const removeTokenConfig = (index: number) => {
    setSettings((prev) => ({
      ...prev,
      tokenConfigs: prev.tokenConfigs.filter((_, i) => i !== index)
    }));
  };

  const handleLanguageChange = (lang: string) => {
    i18n.changeLanguage(lang);
    handleChange('language', lang);
  };

  const handleSubmit = (e: React.FormEvent) => {
    e.preventDefault();
    onSave(settings);
  };

  return (
    <form onSubmit={handleSubmit} className="space-y-6">
      {/* Sprache */}
      <div>
        <label className="block font-medium mb-1">{t('settings.language')}</label>
        <select
          value={settings.language}
          onChange={(e) => handleLanguageChange(e.target.value)}
          className="w-full border p-2"
        >
          <option value="de">{t('settings.language.de')}</option>
          <option value="en">{t('settings.language.en')}</option>
        </select>
      </div>

      {/* Schriftgröße */}
      <div>
        <label className="block font-medium">{t('settings.fontSize')}</label>
        <select
          value={settings.fontSize}
          onChange={(e) => handleChange('fontSize', e.target.value as FontSizeOption)}
          className="w-full border p-2"
        >
          <option value="small">Small</option>
          <option value="normal">Normal</option>
          <option value="large">Large</option>
        </select>
      </div>

      {/* Hintergrundbild */}
      <div>
        <label className="block font-medium">{t('settings.backgroundImageUrl')}</label>
        <input
          type="text"
          value={settings.backgroundImageUrl ?? ''}
          onChange={(e) => handleChange('backgroundImageUrl', e.target.value)}
          className="w-full border p-2"
        />
      </div>

      {/* Report-Optionen */}
      <div>
        <label className="block font-medium">
          <input
            type="checkbox"
            checked={settings.autoSendReports}
            onChange={(e) => handleChange('autoSendReports', e.target.checked)}
            className="mr-2"
          />
          {t('settings.autoSendReports')}
        </label>
      </div>

      <div>
        <label className="block font-medium">
          <input
            type="checkbox"
            checked={settings.downloadOnly}
            onChange={(e) => handleChange('downloadOnly', e.target.checked)}
            className="mr-2"
          />
          {t('settings.downloadOnly')}
        </label>
      </div>

      <div>
        <label className="block font-medium">{t('settings.sendOnDay')}</label>
        <input
          type="number"
          value={settings.sendOnDay}
          onChange={(e) => handleChange('sendOnDay', Number(e.target.value))}
          className="w-full border p-2"
        />
      </div>

      <div>
        <label className="block font-medium">
          <input
            type="checkbox"
            checked={settings.reportWithSignatureField}
            onChange={(e) => handleChange('reportWithSignatureField', e.target.checked)}
            className="mr-2"
          />
          {t('settings.reportWithSignatureField')}
        </label>
      </div>

      {/* Token-Konfiguration */}
      <div>
        <label className="block font-medium">{t('settings.tokenConfigs')}</label>
        {settings.tokenConfigs.map((tc, i) => (
          <div key={i} className="border p-2 mb-2 rounded bg-gray-50">
            <select
              value={tc.loginType}
              onChange={(e) => handleTokenConfigChange(i, 'loginType', e.target.value as LoginMethod)}
              className="mr-2"
            >
              <option value="Passwort">Passwort</option>
              <option value="NFC">NFC</option>
              <option value="QR">QR</option>
              <option value="App">App</option>
              <option value="Terminal">Terminal</option>
            </select>
            <input
              type="text"
              value={tc.validityDuration}
              onChange={(e) => handleTokenConfigChange(i, 'validityDuration', e.target.value)}
              placeholder={t('settings.validityDuration')}
              className="w-32 mr-2"
            />
            <button type="button" onClick={() => removeTokenConfig(i)} className="text-red-600">
              {t('settings.removeToken')}
            </button>
          </div>
        ))}
        <button type="button" onClick={addTokenConfig} className="text-blue-600 underline">
          {t('settings.addToken')}
        </button>
      </div>

      {/* Weitere Optionen */}
      <div>
        <label className="block font-medium">{t('settings.tokenMaxLifetime')}</label>
        <input
          type="text"
          value={settings.tokenMaxLifetime}
          onChange={(e) => handleChange('tokenMaxLifetime', e.target.value)}
          className="w-full border p-2"
        />
      </div>

      <div>
        <label className="block font-medium">
          <input
            type="checkbox"
            checked={settings.qrTokenSingleUse}
            onChange={(e) => handleChange('qrTokenSingleUse', e.target.checked)}
            className="mr-2"
          />
          {t('settings.qrTokenSingleUse')}
        </label>
      </div>

      <div>
        <label className="block font-medium">
          <input
            type="checkbox"
            checked={settings.enableReminder}
            onChange={(e) => handleChange('enableReminder', e.target.checked)}
            className="mr-2"
          />
          {t('settings.enableReminder')}
        </label>
      </div>

      <div>
        <label className="block font-medium">{t('settings.remindAfterDays')}</label>
        <input
          type="number"
          value={settings.remindAfterDays}
          onChange={(e) => handleChange('remindAfterDays', Number(e.target.value))}
          className="w-full border p-2"
        />
      </div>

      <div>
        <label className="block font-medium">{t('settings.errorTypesToCheck')}</label>
        <input
          type="text"
          value={settings.errorTypesToCheck.join(',')}
          onChange={(e) => handleChange('errorTypesToCheck', e.target.value.split(',').map(s => s.trim()))}
          className="w-full border p-2"
        />
      </div>

      <div>
        <label className="block font-medium">{t('settings.holidaySource')}</label>
        <input
          type="text"
          value={settings.holidaySource}
          onChange={(e) => handleChange('holidaySource', e.target.value)}
          className="w-full border p-2"
        />
      </div>

      <div>
        <label className="block font-medium">{t('settings.holidayRegionCode')}</label>
        <input
          type="text"
          value={settings.holidayRegionCode}
          onChange={(e) => handleChange('holidayRegionCode', e.target.value)}
          className="w-full border p-2"
        />
      </div>

      <div>
        <label className="block font-medium">
          <input
            type="checkbox"
            checked={settings.autoSyncHolidays}
            onChange={(e) => handleChange('autoSyncHolidays', e.target.checked)}
            className="mr-2"
          />
          {t('settings.autoSyncHolidays')}
        </label>
      </div>

      <button type="submit" className="bg-blue-600 hover:bg-blue-700 text-white px-6 py-2 rounded">
        {t('settings.save')}
      </button>
    </form>
  );
};

export default SettingsForm;
