// src/components/CreateUserModal.tsx

import { useState, useEffect } from 'react';
import { getToken } from '../utils/auth'; // Token-Helper

interface CreateUserModalProps {
  onClose: () => void;
}

function generateRandomPassword(length = 12) {
  const charset = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789!@#$%^&*()";
  let password = "";
  for (let i = 0; i < length; i++) {
    password += charset[Math.floor(Math.random() * charset.length)];
  }
  return password;
}

export default function CreateUserModal({ onClose }: CreateUserModalProps) {
  const [username, setUsername] = useState('');
  const [name, setName] = useState('');
  const [lastName, setLastName] = useState('');
  const [email, setEmail] = useState('');
  const [role, setRole] = useState('Employee');
  const [passwort, setPasswort] = useState('');
  const [employeeNumber, setEmployeeNumber] = useState('');
  const [isActive, setIsActive] = useState(true);
  const [birthDate, setBirthDate] = useState('');
  const [loading, setLoading] = useState(false);
  const [error, setError] = useState('');

  useEffect(() => {
    setPasswort(generateRandomPassword(12));
  }, []);

  const handleGenerateNewPassword = () => {
    setPasswort(generateRandomPassword(12));
  };

  const handleCreate = async (e: React.FormEvent) => {
    e.preventDefault();
    setError('');
    setLoading(true);

    try {
      const response = await fetch('https://localhost:7123/api/User', {
        method: 'POST',
        headers: {
          'Content-Type': 'application/json',
          'Authorization': `Bearer ${getToken()}`
        },
        body: JSON.stringify({
          name,
          lastName,
          birthDate,
          username,
          email,
          role,
          passwort,
          employeeNumber,
          isActive
        })
      });

      if (!response.ok) {
        const result = await response.json();
        throw new Error(result.message || 'Fehler beim Erstellen.');
      }

      alert('Benutzer erfolgreich erstellt.');
      onClose();
    } catch (err: any) {
      console.error('Fehler beim Erstellen:', err);
      setError(err.message || 'Unbekannter Fehler.');
    } finally {
      setLoading(false);
    }
  };

  return (
    <div className="fixed inset-0 bg-black bg-opacity-50 flex items-center justify-center p-4 z-50">
      <div className="bg-white rounded-lg p-8 w-full max-w-md max-h-[90vh] overflow-y-auto">
        <h2 className="text-xl font-bold mb-4">Benutzer erstellen</h2>

        {error && (
          <div className="bg-red-100 text-red-700 p-2 rounded mb-4 text-sm">
            {error}
          </div>
        )}

        <form onSubmit={handleCreate} className="space-y-4">
          {/* Benutzername */}
            <div>
            <label className="block mb-1 font-medium">Benutzername</label>
            <input
              type="text"
              value={username}
              onChange={e => setUsername(e.target.value)}
              required
              className="w-full border border-gray-300 rounded px-3 py-2"
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
              placeholder="Optional"
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

          {/* Passwort */}
          <div>
            <label className="block mb-1 font-medium">Generiertes Passwort</label>
            <div className="flex space-x-2">
              <input
                type="text"
                value={passwort}
                readOnly
                className="flex-1 border border-gray-300 rounded px-3 py-2 bg-gray-100"
              />
              <button
                type="button"
                onClick={handleGenerateNewPassword}
                className="bg-blue-600 hover:bg-blue-700 text-white px-3 py-2 rounded"
              >
                Neu
              </button>
            </div>
            <p className="text-xs text-gray-500 mt-1">
              Bitte Passwort kopieren und dem Benutzer mitteilen.
            </p>
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
              {loading ? 'Speichern...' : 'Benutzer speichern'}
            </button>
          </div>
        </form>
      </div>
    </div>
  );
}
