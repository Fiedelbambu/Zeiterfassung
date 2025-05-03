import { useEffect, useState } from "react";
import { getToken } from "../utils/auth";
import { useLanguage } from '../i18n/useLanguage';
import { useTranslation } from 'react-i18next';
import { FontSizeOption } from "../types/settingsTypes";


interface TokenConfig {
  loginType: number;
  validityDuration: string;
}

interface SystemSettingsDTO {
  language: string;
  fontSize: number;
  backgroundImageUrl: string;
  autoSendReports: boolean;
  downloadOnly: boolean;
  sendOnDay: number;
  reportWithSignatureField: boolean;
  tokenConfigs: TokenConfig[];
  tokenMaxLifetime: string;
  qrTokenSingleUse: boolean;
  enableReminder: boolean;
  remindAfterDays: number;
  errorTypesToCheck: string[];
  holidaySource: string;
  holidayRegionCode: string;
  autoSyncHolidays: boolean;
}

export default function SystemSettingsPage() {
  const [settings, setSettings] = useState<SystemSettingsDTO | null>(null);
  const [loading, setLoading] = useState(true);
  const [saving, setSaving] = useState(false);
  const [error, setError] = useState("");
  const { setLanguage } = useLanguage();
  const { t } = useTranslation();

  useEffect(() => {
    const fetchSettings = async () => {
      try {
        const res = await fetch("https://localhost:7123/api/settings", {
          headers: { Authorization: `Bearer ${getToken()}` },
        });
        const data = await res.json();
        setSettings(data);
      } catch (e: any) {
        setError("Fehler beim Laden der Systemeinstellungen.");
      } finally {
        setLoading(false);
      }
    };
    fetchSettings();
  }, []);


const handleSave = async () => {
  if (!settings) return;
  setSaving(true);
  try {
    const res = await fetch("https://localhost:7123/api/settings", {
      method: "PUT",
      headers: {
        "Content-Type": "application/json",
        Authorization: `Bearer ${getToken()}`,
      },
      body: JSON.stringify(settings),
    });

    if (!res.ok) throw new Error("Fehler beim Speichern.");

    // Sprache direkt umstellen
    setLanguage(settings.language);
    localStorage.setItem('language', settings.language);

    alert("Einstellungen gespeichert.");
  } catch (e: any) {
    alert(e.message);
  } finally {
    setSaving(false);
  }
};


  if (loading) return <p>Lade Einstellungen...</p>;
  if (error) return <p className="text-red-500">{error}</p>;
  if (!settings) return null;

  return (
    <div className="max-w-3xl mx-auto p-6 bg-white rounded shadow">
      <h2 className="text-2xl font-bold mb-4">Systemeinstellungen</h2>

      {/* Sprache */}
      <label>Sprache</label>
      <select
        value={settings.language}
        onChange={(e) => setSettings({ ...settings, language: e.target.value })}
        className="w-full border p-2 mb-4"
      >
        <option value="de">Deutsch</option>
        <option value="en">Englisch</option>
        <option value="ooe">Oberösterreichisch</option>
      </select>

  
{/* Schriftgröße */}
<div>
  <label>{t('settings.fontSizeLabel')}</label>
  <select
    value={settings.fontSize}
    onChange={(e) =>
      setSettings({ ...settings, fontSize: parseInt(e.target.value) as FontSizeOption })
    }
    className="w-full border p-2 mb-4"
  >
    <option value={1}>{t("settings.fontSizeOptions.small")}</option>
    <option value={2}>{t("settings.fontSizeOptions.normal")}</option>
    <option value={3}>{t("settings.fontSizeOptions.large")}</option>
  </select>
</div>


      {/* Hintergrundbild */}
      <label>Hintergrundbild-URL</label>
      <input
        type="text"
        value={settings.backgroundImageUrl || ""}
        onChange={(e) =>
          setSettings({ ...settings, backgroundImageUrl: e.target.value })
        }
        className="w-full border p-2 mb-4"
      />

      {/* Auto-Versand */}
      <label>
        <input
          type="checkbox"
          checked={settings.autoSendReports}
          onChange={(e) =>
            setSettings({ ...settings, autoSendReports: e.target.checked })
          }
          className="mr-2"
        />
        Berichte automatisch senden
      </label>

      {/* Download-Only */}
      <div>
        <label>
          <input
            type="checkbox"
            checked={settings.downloadOnly}
            onChange={(e) =>
              setSettings({ ...settings, downloadOnly: e.target.checked })
            }
            className="mr-2"
          />
          Nur Download
        </label>
      </div>

      {/* Send-On-Day */}
      <label>Versandtag im Monat</label>
      <input
        type="number"
        min={0}
        max={31}
        value={settings.sendOnDay}
        onChange={(e) =>
          setSettings({ ...settings, sendOnDay: parseInt(e.target.value) })
        }
        className="w-full border p-2 mb-4"
      />

      {/* Signaturfeld */}
      <label>
        <input
          type="checkbox"
          checked={settings.reportWithSignatureField}
          onChange={(e) =>
            setSettings({
              ...settings,
              reportWithSignatureField: e.target.checked,
            })
          }
          className="mr-2"
        />
        Signaturfeld im Bericht anzeigen
      </label>

      {/* Token-Lifetime */}
      <label className="block mt-4">TokenMaxLifetime (02:00:00)</label>
      <input
        type="text"
        value={settings.tokenMaxLifetime}
        onChange={(e) =>
          setSettings({ ...settings, tokenMaxLifetime: e.target.value })
        }
        className="w-full border p-2 mb-4"
      />

      {/* QR Single Use */}
      <label>
        <input
          type="checkbox"
          checked={settings.qrTokenSingleUse}
          onChange={(e) =>
            setSettings({ ...settings, qrTokenSingleUse: e.target.checked })
          }
          className="mr-2"
        />
        QR-Token nur einmal gültig
      </label>

      {/* Erinnerung */}
      <div className="mt-4">
        <label>
          <input
            type="checkbox"
            checked={settings.enableReminder}
            onChange={(e) =>
              setSettings({ ...settings, enableReminder: e.target.checked })
            }
            className="mr-2"
          />
          Erinnerung aktivieren
        </label>
      </div>

      {/* Erinnerungstage */}
      <label>Tage bis zur Erinnerung</label>
      <input
        type="number"
        value={settings.remindAfterDays}
        onChange={(e) =>
          setSettings({ ...settings, remindAfterDays: parseInt(e.target.value) })
        }
        className="w-full border p-2 mb-4"
      />

      {/* Fehlerprüfung */}
      <label>Zu prüfende Fehlertypen</label>
      <input
        type="text"
        value={settings.errorTypesToCheck.join(",")}
        onChange={(e) =>
          setSettings({
            ...settings,
            errorTypesToCheck: e.target.value
              .split(",")
              .map((s) => s.trim())
              .filter((x) => x),
          })
        }
        placeholder="z. B. NurKommen,KeinePauseEnde"
        className="w-full border p-2 mb-4"
      />

      {/* Feiertagskonfiguration */}
      <label>Feiertagsquelle</label>
      <select
        value={settings.holidaySource}
        onChange={(e) =>
          setSettings({ ...settings, holidaySource: e.target.value })
        }
        className="w-full border p-2 mb-4"
      >
        <option value="API">Automatisch (API)</option>
        <option value="Manual">Manuell</option>
      </select>

      <label>Feiertagsregion</label>
      <input
        type="text"
        value={settings.holidayRegionCode}
        onChange={(e) =>
          setSettings({ ...settings, holidayRegionCode: e.target.value })
        }
        className="w-full border p-2 mb-4"
      />

      <label>
        <input
          type="checkbox"
          checked={settings.autoSyncHolidays}
          onChange={(e) =>
            setSettings({ ...settings, autoSyncHolidays: e.target.checked })
          }
          className="mr-2"
        />
        Feiertage automatisch synchronisieren
      </label>

      {/* Speichern */}
      <div className="mt-6">
        <button
          onClick={handleSave}
          disabled={saving}
          className="bg-blue-600 hover:bg-blue-700 text-white px-6 py-2 rounded"
        >
          {saving ? "Speichern..." : "Einstellungen speichern"}
        </button>
      </div>
    </div>
  );
}
