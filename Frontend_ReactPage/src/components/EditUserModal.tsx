// src/components/EditUserModal.tsx

import { useState } from 'react';
import { getToken } from '../utils/auth';

interface User {
  id: string;
  username: string;
  name: string;
  lastName: string;
  email: string | null;
  role: string;
  employeeNumber: string | null;
  birthDate: string;
  aktiv: boolean;
  erstelltAm: string;
}

interface EditUserModalProps {
  user: User;
  onClose: () => void;
}

export default function EditUserModal({ user, onClose }: EditUserModalProps) {
  const [name, setName] = useState(user.name);
  const [lastName, setLastName] = useState(user.lastName);
  const [email, setEmail] = useState(user.email ?? '');
  const [role, setRole] = useState(user.role);
  const [birthDate, setBirthDate] = useState(user.birthDate ? user.birthDate.substring(0, 10) : '');
  const [employeeNumber, setEmployeeNumber] = useState(user.employeeNumber ?? '');
  const [isActive, setIsActive] = useState(user.aktiv);
  const [loading, setLoading] = useState(false);
  const [error, setError] = useState('');

  const handleSave = async (e: React.FormEvent) => {
    e.preventDefault();
    setError('');
    setLoading(true);

    try {
      const response = await fetch(`https://localhost:7123/api/UserAdmin/${user.id}`, {
        method: 'PUT',
        headers: {
          'Content-Type': 'application/json',
          'Authorization': `Bearer ${getToken()}`
        },
        body: JSON.stringify({
          name,
          lastName,
          email,
          role,
          birthDate,
          employeeNumber,
          aktiv: isActive
        })
      });

      if (!response.ok) {
        let errorMessage = 'Fehler beim Speichern.';
        try {
          const result = await response.json();
          errorMessage = result.message || errorMessage;
        } catch (err) {
          // Keine JSON-Antwort vorhanden → nichts tun, Standard-Fehlermeldung behalten
        }
        throw new Error(errorMessage);
      }
      

      alert('Benutzerdaten erfolgreich aktualisiert.');
      onClose();
    } catch (err: any) {
      console.error('Fehler beim Aktualisieren:', err);
      setError(err.message || 'Unbekannter Fehler.');
    } finally {
      setLoading(false);
    }
  };

  return (
    <div className="fixed inset-0 bg-black bg-opacity-50 flex items-center justify-center p-4 z-50">
      <div className="bg-white rounded-lg p-8 w-full max-w-md max-h-[90vh] overflow-y-auto">
        <h2 className="text-xl font-bold mb-4">Benutzer bearbeiten</h2>

        {error && (
          <div className="bg-red-100 text-red-700 p-2 rounded mb-4 text-sm">
            {error}
          </div>
        )}

        <form onSubmit={handleSave} className="space-y-4">
          {/* Benutzername (nur Anzeige) */}
          <div>
            <label className="block mb-1 font-medium">Benutzername</label>
            <input
              type="text"
              value={user.username}
              disabled
              className="w-full border border-gray-300 rounded px-3 py-2 bg-gray-100"
            />
          </div>

          {/* Name */}
          <div>
            <label className="block mb-1 font-medium">Name</label>
            <input
              type="text"
              value={name}
              onChange={e => setName(e.target.value)}
              required
              className="w-full border border-gray-300 rounded px-3 py-2"
            />
          </div>

          {/* Nachname */}
          <div>
            <label className="block mb-1 font-medium">Nachname</label>
            <input
              type="text"
              value={lastName}
              onChange={e => setLastName(e.target.value)}
              required
              className="w-full border border-gray-300 rounded px-3 py-2"
            />
          </div>

          {/* Geburtsdatum */}
          <div>
            <label className="block mb-1 font-medium">Geburtsdatum</label>
            <input
              type="date"
              value={birthDate}
              onChange={e => setBirthDate(e.target.value)}
              required
              className="w-full border border-gray-300 rounded px-3 py-2"
            />
          </div>

          {/* Mitarbeiter-Nummer */}
          <div>
            <label className="block mb-1 font-medium">Mitarbeiter-Nummer</label>
            <input
              type="text"
              value={employeeNumber}
              onChange={e => setEmployeeNumber(e.target.value)}
              className="w-full border border-gray-300 rounded px-3 py-2"
            />
          </div>

          {/* E-Mail */}
          <div>
            <label className="block mb-1 font-medium">E-Mail (optional)</label>
            <input
              type="email"
              value={email}
              onChange={e => setEmail(e.target.value)}
              className="w-full border border-gray-300 rounded px-3 py-2"
            />
          </div>

          {/* Rolle */}
          <div>
            <label className="block mb-1 font-medium">Rolle</label>
            <select
              value={role}
              onChange={e => setRole(e.target.value)}
              className="w-full border border-gray-300 rounded px-3 py-2"
            >
              <option value="Employee">Mitarbeiter</option>
              <option value="Admin">Admin</option>
            </select>
          </div>

          {/* Aktiv/Inaktiv */}
          <div className="flex items-center">
            <input
              id="active"
              type="checkbox"
              checked={isActive}
              onChange={e => setIsActive(e.target.checked)}
              className="h-4 w-4 text-blue-600 border-gray-300 rounded"
            />
            <label htmlFor="active" className="ml-2 block text-sm text-gray-700">
              Benutzer aktiv
            </label>
          </div>

          {/* Aktionen */}
          <div className="flex justify-end space-x-4 mt-6">
            <button
              type="button"
              onClick={onClose}
              className="bg-gray-300 hover:bg-gray-400 text-gray-800 py-2 px-4 rounded"
            >
              Abbrechen
            </button>

            <button
              type="submit"
              disabled={loading}
              className="bg-green-600 hover:bg-green-700 text-white py-2 px-4 rounded"
            >
              {loading ? 'Speichern...' : 'Änderungen speichern'}
            </button>
          </div>
        </form>
      </div>
    </div>
  );
}
