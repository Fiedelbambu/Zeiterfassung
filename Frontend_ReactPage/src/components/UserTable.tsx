// src/components/UserTable.tsx

import { useEffect, useState } from 'react';
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

interface UserTableProps {
  onEditUser: (user: User) => void;
  reloadRef: React.RefObject<() => void>; // ✅ Neu
}

export default function UserTable({ onEditUser, reloadRef }: UserTableProps) {
  const [users, setUsers] = useState<User[]>([]);
  const [loading, setLoading] = useState(false);
  const [error, setError] = useState('');

  const fetchUsers = async () => {
    setLoading(true);
    try {
      const response = await fetch('https://localhost:7123/api/UserAdmin', {
        headers: {
          Authorization: `Bearer ${getToken()}`
        }
      });
      if (!response.ok) throw new Error('Fehler beim Abrufen der Benutzer.');
      const data = await response.json();
      setUsers(data);
    } catch (err: any) {
      console.error('Fehler beim Abrufen:', err);
      setError(err.message || 'Unbekannter Fehler.');
    } finally {
      setLoading(false);
    }
  };

  useEffect(() => {
    fetchUsers();
    reloadRef.current = fetchUsers; // <-- Setzt den aktuellen reload
  }, [reloadRef]);

  const handleEdit = (user: User) => {
    onEditUser(user);
  };
  
  useEffect(() => {
    const fetchUsers = async () => {
      setLoading(true);
      try {
        const response = await fetch('https://localhost:7123/api/UserAdmin', {
          headers: {
            'Authorization': `Bearer ${getToken()}`
          }
        });
        if (!response.ok) throw new Error('Fehler beim Abrufen der Benutzer.');
        const data = await response.json();
        setUsers(data);
      } catch (err: any) {
        console.error('Fehler beim Abrufen:', err);
        setError(err.message || 'Unbekannter Fehler.');
      } finally {
        setLoading(false);
      }
    };
    fetchUsers();
  }, []);

  return (
    <div className="bg-white shadow rounded overflow-x-auto">
      {loading && <p className="p-4">Lade Benutzer...</p>}
      {error && <p className="p-4 text-red-600">{error}</p>}

      {!loading && !error && (
      <table className="min-w-full">
      <thead className="bg-gray-100">
        <tr>
          <th className="p-3 text-left font-semibold">Benutzername</th>
          <th className="p-3 text-left font-semibold">Vorname</th>
          <th className="p-3 text-left font-semibold">Nachname</th>
          <th className="p-3 text-left font-semibold">E-Mail</th>
          <th className="p-3 text-left font-semibold">Geburtsdatum</th>
          <th className="p-3 text-left font-semibold">Mitarbeiter-Nr.</th>
          <th className="p-3 text-left font-semibold">Rolle</th>
          <th className="p-3 text-left font-semibold">Status</th>
          <th className="p-3 text-left font-semibold">Erstellt am</th>
          <th className="p-3 text-left font-semibold">Aktionen</th> {/* NEU */}
        </tr>
      </thead>
    
      <tbody>
        {users.map(user => (
          <tr key={user.id} className="border-b">
            <td className="p-3">{user.username}</td>
            <td className="p-3">{user.name}</td>
            <td className="p-3">{user.lastName}</td>
            <td className="p-3">{user.email || '-'}</td>
            <td className="p-3">{user.birthDate ? new Date(user.birthDate).toLocaleDateString() : '-'}</td>
            <td className="p-3">{user.employeeNumber || '-'}</td>
            <td className="p-3">{user.role}</td>
            <td className="p-3">
              {user.aktiv ? (
                <span className="text-green-600 font-semibold">Aktiv</span>
              ) : (
                <span className="text-red-600 font-semibold">Inaktiv</span>
              )}
            </td>
            <td className="p-3">{new Date(user.erstelltAm).toLocaleDateString()}</td>
            <td className="p-3">
              <button
                onClick={() => handleEdit(user)}
                className="text-blue-600 hover:underline"
              >
                Bearbeiten
              </button>
            </td> 
          </tr>
        ))}
      </tbody>
    </table>    
      )}
    </div>
      );



  
}
