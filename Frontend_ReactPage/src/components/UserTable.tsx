// src/components/UserTable.tsx

import { useEffect, useState } from "react";
import { getToken } from "../utils/auth";
import EditEmployeeModal from "./EditEmployeeModal";

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

  abteilung?: string;
  telefon?: string;
  standort?: string;
}


interface UserTableProps {
  onEditUser: (user: User) => void;
  reloadRef: React.RefObject<() => void>; // Funktion zum Neuladen der Tabelle
}

export default function UserTable({ onEditUser, reloadRef }: UserTableProps) {
  const [users, setUsers] = useState<User[]>([]);
  const [loading, setLoading] = useState(false);
  const [error, setError] = useState("");
  const [openUserId, setOpenUserId] = useState<string | null>(null);
  const [searchTerm, setSearchTerm] = useState("");
  const roles = ["Admin", "Employee"]; // später erweiterbar
  const [editingEmployeeUser, setEditingEmployeeUser] = useState<User | null>(
    null
  );

  const toggleMenu = (userId: string) => {
    setOpenUserId((prev) => (prev === userId ? null : userId));
  };

  const filteredUsers = users.filter(
    (u) =>
      u.username.toLowerCase().includes(searchTerm.toLowerCase()) ||
      u.name.toLowerCase().includes(searchTerm.toLowerCase()) ||
      u.lastName.toLowerCase().includes(searchTerm.toLowerCase()) ||
      (u.email || "").toLowerCase().includes(searchTerm.toLowerCase())
  );

  const handleRoleChange = async (user: User, neueRolle: string) => {
    if (neueRolle === user.role) return; // keine Änderung nötig

    try {
      const response = await fetch(
        `https://localhost:7123/api/UserAdmin/${user.id}`,
        {
          method: "PUT",
          headers: {
            "Content-Type": "application/json",
            Authorization: `Bearer ${getToken()}`,
          },
          body: JSON.stringify({
            name: user.name,
            lastName: user.lastName,
            email: user.email,
            employeeNumber: user.employeeNumber,
            birthDate: user.birthDate,
            aktiv: user.aktiv,
            role: neueRolle,
          }),
        }
      );

      if (response.ok) {
        alert(`Rolle erfolgreich geändert zu ${neueRolle}`);
        fetchUsers(); // reload Tabelle
      } else {
        const result = await response.json();
        alert(result.message || "Fehler beim Rollenwechsel.");
      }
    } catch (err) {
      console.error(err);
      alert("Fehler beim Rollenwechsel.");
    }
  };

  const handleEmployeeEdit = (user: User) => {
    alert(
      `Mitarbeiterdaten von ${user.username} bearbeiten (noch nicht implementiert).`
    );
  };

  const fetchUsers = async () => {
    setLoading(true);
    try {
      const response = await fetch("https://localhost:7123/api/UserAdmin", {
        headers: {
          Authorization: `Bearer ${getToken()}`,
        },
      });
      if (!response.ok) throw new Error("Fehler beim Abrufen der Benutzer.");
      const data = await response.json();
      setUsers(data);
    } catch (err: any) {
      console.error("Fehler beim Abrufen:", err);
      setError(err.message || "Unbekannter Fehler.");
    } finally {
      setLoading(false);
    }
  };

  const handleDelete = async (user: User) => {
    const confirmed = window.confirm(
      `Benutzer "${user.username}" wirklich löschen?`
    );
    if (!confirmed) return;

    try {
      const response = await fetch(
        `https://localhost:7123/api/UserAdmin/${user.id}`,
        {
          method: "DELETE",
          headers: {
            Authorization: `Bearer ${getToken()}`,
          },
        }
      );

      if (response.status === 204 || response.status === 200) {
        alert("Benutzer gelöscht.");
        fetchUsers(); // Tabelle neu laden
      } else {
        const result = await response.json();
        alert(result.message || "Fehler beim Löschen.");
      }
    } catch (err) {
      console.error("Fehler beim Löschen:", err);
      alert("Fehler beim Löschen.");
    }
  };

  useEffect(() => {
    fetchUsers();
    reloadRef.current = fetchUsers; // <-- Setzt den aktuellen reload
  }, [reloadRef]);

  const handleEdit = (user: User) => {
    onEditUser(user);
  };

  return (
    <div className="bg-white shadow rounded overflow-x-auto">
      {loading && <p className="p-4">Lade Benutzer...</p>}
      {error && <p className="p-4 text-red-600">{error}</p>}

      {/*Suchfeld */}
      <input
        type="text"
        placeholder="Suche nach Name, Benutzername, E-Mail..."
        value={searchTerm}
        onChange={(e) => setSearchTerm(e.target.value)}
        className="mb-4 p-2 border rounded w-full max-w-md"
      />

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
              <th className="p-3 text-left font-semibold">Abteilung</th>
              <th className="p-3 text-left font-semibold">Telefon</th>
              <th className="p-3 text-left font-semibold">Standort</th>
              <th className="p-3 text-left font-semibold">Rolle</th>
              <th className="p-3 text-left font-semibold">Status</th>
              <th className="p-3 text-left font-semibold">Erstellt am</th>
              <th className="p-3 text-left font-semibold">Aktionen</th>
            </tr>
          </thead>

          <tbody>
            {filteredUsers.map((user) => (
              <tr key={user.id} className="border-b">
                <td className="p-3">{user.username}</td>
                <td className="p-3">{user.name}</td>
                <td className="p-3">{user.lastName}</td>
                <td className="p-3">{user.email || "-"}</td>
                <td className="p-3">
                  {user.birthDate
                    ? new Date(user.birthDate).toLocaleDateString()
                    : "-"}
                </td>
                <td className="p-3">{user.employeeNumber || "-"}</td>
                <td className="p-3">{user.abteilung || "-"}</td>
                <td className="p-3">{user.telefon || "-"}</td>
                <td className="p-3">{user.standort || "-"}</td>
                <td className="p-3">{user.role}</td>
                <td className="p-3">
                  {user.aktiv ? (
                    <span className="text-green-600 font-semibold">Aktiv</span>
                  ) : (
                    <span className="text-red-600 font-semibold">Inaktiv</span>
                  )}
                </td>
                <td className="p-3">
                  {new Date(user.erstelltAm).toLocaleDateString()}
                </td>
                <td className="p-3 relative">
                  <button
                    onClick={() => toggleMenu(user.id)}
                    className="text-blue-600 hover:underline"
                  >
                    Aktionen ▾
                  </button>
                  {openUserId === user.id && (
                    <div className="absolute right-0 mt-1 w-48 rounded-md shadow-lg bg-white ring-1 ring-black ring-opacity-5 z-10">
                      <ul className="py-1 text-sm text-gray-700">
                        <li>
                          <button
                            onClick={() => {
                              handleEdit(user);
                              setOpenUserId(null);
                            }}
                            className="w-full text-left px-4 py-2 hover:bg-gray-100"
                          >
                            Benutzer bearbeiten
                          </button>
                        </li>
                        <li>
                          <button
                            onClick={() => {
                              handleEmployeeEdit(user);
                              setOpenUserId(null);
                            }}
                            className="w-full text-left px-4 py-2 hover:bg-gray-100"
                          >
                            Mitarbeiterdaten
                          </button>
                        </li>
                        <li>
                          <button
                            onClick={() => {
                              handleDelete(user);
                              setOpenUserId(null);
                            }}
                            className="w-full text-left px-4 py-2 text-red-600 hover:bg-red-50"
                          >
                            Löschen
                          </button>
                        </li>
                        <li className="px-4 py-2">
                          <label className="block text-sm text-gray-700 mb-1">
                            Rolle ändern:
                          </label>
                          <select
                            value={user.role}
                            onChange={(e) =>
                              handleRoleChange(user, e.target.value)
                            }
                            className="w-full border border-gray-300 rounded px-2 py-1 text-sm"
                          >
                            {roles.map((role) => (
                              <option key={role} value={role}>
                                {role}
                              </option>
                            ))}
                          </select>
                        </li>
                      </ul>
                    </div>
                  )}
                </td>
              </tr>
            ))}
          </tbody>
        </table>
      )}

      {editingEmployeeUser && (
        <EditEmployeeModal
          user={editingEmployeeUser}
          onClose={() => setEditingEmployeeUser(null)}
          onSuccess={() => {
            setEditingEmployeeUser(null);
            fetchUsers(); // Tabelle neu laden
          }}
        />
      )}
    </div>
  );
}
