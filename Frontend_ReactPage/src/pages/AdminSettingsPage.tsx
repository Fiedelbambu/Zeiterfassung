import { useEffect, useState } from "react";
import { getToken } from "../utils/auth";

interface AdminSettingsDTO {
  language: string;
  fontSize: string;
  backgroundImageEnabled: boolean;
  autoSendReports: boolean;
}

export default function AdminSettingsPage() {
  const [settings, setSettings] = useState<AdminSettingsDTO>({
    language: "de",
    fontSize: "normal",
    backgroundImageEnabled: false,
    autoSendReports: false,
  });

  const [loading, setLoading] = useState(true);
  const [error, setError] = useState("");
  const [saving, setSaving] = useState(false);

  useEffect(() => {
    const fetchSettings = async () => {
      try {
        const response = await fetch("https://localhost:7123/api/admin/settings", {
          headers: {
            Authorization: `Bearer ${getToken()}`,
          },
        });

        if (!response.ok) throw new Error("Fehler beim Laden der Einstellungen.");
        const data = await response.json();
        setSettings(data);
      } catch (err: any) {
        setError(err.message || "Unbekannter Fehler.");
      } finally {
        setLoading(false);
      }
    };

    fetchSettings();
  }, []);

  const handleSave = async () => {
    setSaving(true);
    try {
      const response = await fetch("https://localhost:7123/api/admin/settings", {
        method: "PUT",
        headers: {
          "Content-Type": "application/json",
          Authorization: `Bearer ${getToken()}`,
        },
        body: JSON.stringify(settings),
      });

      if (!response.ok) {
        const result = await response.json();
        throw new Error(result.message || "Fehler beim Speichern.");
      }

      alert("Einstellungen erfolgreich gespeichert.");
    } catch (err: any) {
      alert(err.message || "Unbekannter Fehler.");
    } finally {
      setSaving(false);
    }
  };

  if (loading) return <p className="p-4">Lade Einstellungen...</p>;
  if (error) return <p className="p-4 text-red-600">{error}</p>;

  return (
    <div className="max-w-xl mx-auto p-6 bg-white rounded shadow">
      <h1 className="text-2xl font-bold mb-6">Systemeinstellungen</h1>

      {/* Sprache */}
      <div className="mb-4">
        <label className="block font-medium mb-1">Sprache</label>
        <select
          value={settings.language}
          onChange={(e) => setSettings({ ...settings, language: e.target.value })}
          className="w-full border px-3 py-2 rounded"
        >
          <option value="de">Deutsch</option>
          <option value="en">Englisch</option>
        </select>
      </div>

      {/* Schriftgröße */}
      <div className="mb-4">
        <label className="block font-medium mb-1">Schriftgröße</label>
        <select
          value={settings.fontSize}
          onChange={(e) => setSettings({ ...settings, fontSize: e.target.value })}
          className="w-full border px-3 py-2 rounded"
        >
          <option value="small">Klein</option>
          <option value="normal">Normal</option>
          <option value="large">Groß</option>
        </select>
      </div>

      {/* Hintergrundbild */}
      <div className="mb-4">
        <label className="inline-flex items-center">
          <input
            type="checkbox"
            checked={settings.backgroundImageEnabled}
            onChange={(e) =>
              setSettings({ ...settings, backgroundImageEnabled: e.target.checked })
            }
            className="mr-2"
          />
          Hintergrundbild aktivieren
        </label>
      </div>

      {/* Automatischer Versand */}
      <div className="mb-6">
        <label className="inline-flex items-center">
          <input
            type="checkbox"
            checked={settings.autoSendReports}
            onChange={(e) =>
              setSettings({ ...settings, autoSendReports: e.target.checked })
            }
            className="mr-2"
          />
          Stundenzettel automatisch versenden
        </label>
      </div>

      <button
        onClick={handleSave}
        disabled={saving}
        className="bg-blue-600 hover:bg-blue-700 text-white px-4 py-2 rounded"
      >
        {saving ? "Speichern..." : "Einstellungen speichern"}
      </button>
    </div>
  );
}
