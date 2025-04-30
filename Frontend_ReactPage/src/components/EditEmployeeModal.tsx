import React, { useState } from 'react';
import { getToken } from '../utils/auth';


interface EditEmployeeModalProps {
    onClose: () => void;
    user: {
      id: string;
      username: string;
      abteilung?: string;
      telefon?: string;
      standort?: string;
    };
    onSuccess?: () => void; // Für Reload
  }
  

export default function EditEmployeeModal({ user, onClose }: EditEmployeeModalProps) {
  const [abteilung, setAbteilung] = useState(user.abteilung || '');
  const [telefon, setTelefon] = useState(user.telefon || '');
  const [standort, setStandort] = useState(user.standort || '');

  const handleSave = async () => {
    try {
      const response = await fetch(`https://localhost:7123/api/UserAdmin/${user.id}/employee`, {
        method: 'PUT',
        headers: {
          'Content-Type': 'application/json',
          Authorization: `Bearer ${getToken()}`
        },
        body: JSON.stringify({
          abteilung,
          telefon,
          standort
        })
      });
  
      if (!response.ok) {
        const result = await response.json();
        throw new Error(result.message || 'Fehler beim Speichern der Mitarbeiterdaten.');
      }
  
      alert('Mitarbeiterdaten gespeichert.');
      onClose();
      if (onSuccess) onSuccess(); // Reload
    } catch (err: any) {
      alert(err.message || 'Unbekannter Fehler beim Speichern.');
      console.error('Fehler beim Speichern:', err);
    }
  };
  

  return (
    <div className="fixed inset-0 bg-black bg-opacity-50 flex items-center justify-center z-50">
      <div className="bg-white rounded-lg p-6 w-full max-w-md shadow-lg">
        <h2 className="text-lg font-semibold mb-4">Mitarbeiterdaten bearbeiten</h2>

        <div className="space-y-3">
          <div>
            <label className="block mb-1 font-medium">Abteilung</label>
            <input
              type="text"
              value={abteilung}
              onChange={(e) => setAbteilung(e.target.value)}
              className="w-full border border-gray-300 rounded px-3 py-2"
            />
          </div>

          <div>
            <label className="block mb-1 font-medium">Telefon</label>
            <input
              type="text"
              value={telefon}
              onChange={(e) => setTelefon(e.target.value)}
              className="w-full border border-gray-300 rounded px-3 py-2"
            />
          </div>

          <div>
            <label className="block mb-1 font-medium">Standort</label>
            <input
              type="text"
              value={standort}
              onChange={(e) => setStandort(e.target.value)}
              className="w-full border border-gray-300 rounded px-3 py-2"
            />
          </div>
        </div>

        <div className="flex justify-end mt-6 space-x-2">
          <button
            onClick={onClose}
            className="px-4 py-2 bg-gray-200 hover:bg-gray-300 rounded"
          >
            Abbrechen
          </button>
          <button
            onClick={handleSave}
            className="px-4 py-2 bg-blue-600 text-white hover:bg-blue-700 rounded"
          >
            Speichern
          </button>
        </div>
      </div>
    </div>
  );
}
