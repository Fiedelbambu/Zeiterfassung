// src/components/UserTable.tsx

import { useEffect, useState } from "react";
import { getToken } from "../utils/auth";
import EditEmployeeModal from "./EditEmployeeModal";
import ActionDropdownPortal from './ActionDropdownPortal';
import { useRef } from 'react';

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
  const menuAnchorRef = useRef<HTMLTableCellElement>(null);
  const [loading, setLoading] = useState(false);
  const [error, setError] = useState("");
  const [openUserId, setOpenUserId] = useState<string | null>(null);
  const [searchTerm, setSearchTerm] = useState("");
  const [sortField, setSortField] = useState<keyof User>("username");
  const [sortDirection, setSortDirection] = useState<"asc" | "desc">("asc");
  const [editingUser, setEditingUser] = useState<User | null>(null);

  
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

  const sortedUsers = [...filteredUsers].sort((a, b) => {
    if (sortField === "birthDate" || sortField === "erstelltAm") {
      return sortDirection === "asc"
        ? new Date(a[sortField] ?? "").getTime() - new Date(b[sortField] ?? "").getTime()
        : new Date(b[sortField] ?? "").getTime() - new Date(a[sortField] ?? "").getTime();
    }
  
    const aVal = (a[sortField] ?? "").toString().toLowerCase();
    const bVal = (b[sortField] ?? "").toString().toLowerCase();
  
    return sortDirection === "asc"
      ? aVal.localeCompare(bVal)
      : bVal.localeCompare(aVal);
  });
  
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
<div className="bg-white shadow rounded overflow-x-auto max-h-[80vh] overflow-y-auto">
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
            {[
              { label: "Benutzername", field: "username" },
              { label: "Vorname", field: "name" },
              { label: "Nachname", field: "lastName" },
              { label: "E-Mail", field: "email" },
              { label: "Geburtsdatum", field: "birthDate" },
              { label: "Mitarbeiter-Nr.", field: "employeeNumber" },
              { label: "Abteilung", field: "abteilung" },
              { label: "Telefon", field: "telefon" },
              { label: "Standort", field: "standort" },
              { label: "Rolle", field: "role" },
              { label: "Status", field: "aktiv" },
              { label: "Erstellt am", field: "erstelltAm" }
            ].map(({ label, field }) => (
              <th
                key={field}
                className="p-3 text-left font-semibold cursor-pointer"
                onClick={() => {
                  if (sortField === field) {
                    setSortDirection((prev) => (prev === "asc" ? "desc" : "asc"));
                  } else {
                    setSortField(field as keyof User);
                    setSortDirection("asc");
                  }
                }}
              >
                {label} {sortField === field && (sortDirection === "asc" ? "▲" : "▼")}
              </th>
            ))}
            <th className="p-3 text-left font-semibold">Aktionen</th>
          </tr>
        </thead>
          

          <tbody>
          {sortedUsers.map((user) => (
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
                <td className="p-3 relative" ref={menuAnchorRef}>
  <button
    onClick={() => toggleMenu(user.id)}
    className="text-blue-600 hover:underline"
  >
    Aktionen ▾
  </button>

  {openUserId === user.id && (
    <ActionDropdownPortal anchorRef={menuAnchorRef} onClose={() => setOpenUserId(null)}>
      <ul className="min-w-[160px] py-1 text-sm">
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
              setEditingEmployeeUser(user);
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
      </ul>
    </ActionDropdownPortal>
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
