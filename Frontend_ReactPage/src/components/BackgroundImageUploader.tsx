import React, { useState } from "react";
import { getToken } from "../utils/auth";

interface Props {
  currentUrl?: string;
  onUploaded?: (newUrl: string) => void; // Optionaler Callback
}

const BackgroundImageUploader = ({ currentUrl, onUploaded }: Props) => {
  const [uploading, setUploading] = useState(false);
  const [error, setError] = useState<string | null>(null);

  const handleFileChange = async (event: React.ChangeEvent<HTMLInputElement>) => {
    const file = event.target.files?.[0];
    if (!file) return;

    setUploading(true);
    setError(null);

    const formData = new FormData();
    formData.append("file", file);

    try {
      const res = await fetch("https://localhost:7123/api/settings/upload-background", {
        method: "POST",
        headers: {
          Authorization: `Bearer ${getToken()}`
        },
        body: formData
      });

      if (!res.ok) throw new Error("Upload fehlgeschlagen.");

      const data = await res.json();
      const url = data.url;

      // Direkt als Hintergrundbild setzen
      document.body.style.backgroundImage = `url(${url})`;
      document.body.style.backgroundSize = "cover";
      document.body.style.backgroundRepeat = "no-repeat";
      document.body.style.backgroundPosition = "center center";

      // Optional: Callback für Speicherung in Settings
      if (onUploaded) onUploaded(url);
    } catch (e: any) {
      setError(e.message || "Unbekannter Fehler");
    } finally {
      setUploading(false);
    }
  };

  return (
    <div>
      {currentUrl && (
        <img src={currentUrl} alt="Aktuelles Hintergrundbild" className="w-32 h-auto mb-2 border" />
      )}
      <input type="file" accept="image/*" onChange={handleFileChange} />
      {uploading && <p>Hochladen...</p>}
      {error && <p className="text-red-600">{error}</p>}
    </div>
  );
};

export default BackgroundImageUploader;
