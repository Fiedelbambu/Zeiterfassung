// src/pages/UsersPage.tsx
import { useState, useRef } from 'react';
import UserTable from '../components/UserTable';
import CreateUserModal from '../components/CreateUserModal';
import EditUserModal from '../components/EditUserModal'; 

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

export default function UsersPage() {
  const [showCreateModal, setShowCreateModal] = useState(false);
  const [editingUser, setEditingUser] = useState<User | null>(null); // User, den wir bearbeiten wollen
  const reloadUsersRef = useRef<() => void>(() => {});
  
  const handleEditUser = (user: User) => {
    setEditingUser(user);
  };

  const handleCloseEditModal = () => {
    setEditingUser(null);
    if (reloadUsersRef.current) {
      reloadUsersRef.current(); //  Tabelle neu laden
    }
  };

  return (
    <div className="p-8 overflow-y-auto max-h-[90vh]">
      <div className="flex justify-between items-center mb-6">
        <h1 className="text-2xl font-bold">Benutzerverwaltung</h1>
        <button
          onClick={() => setShowCreateModal(true)}
          className="bg-blue-600 hover:bg-blue-700 text-white py-2 px-4 rounded"
        >
          Benutzer anlegen
        </button>
      </div>

      <UserTable onEditUser={handleEditUser} reloadRef={reloadUsersRef} />

      {showCreateModal && (
  <CreateUserModal
    onClose={() => {
      setShowCreateModal(false);
      if (reloadUsersRef.current) {
        reloadUsersRef.current(); //  Tabelle neu laden
      }
    }}
  />
)}


      {editingUser && (
        <EditUserModal user={editingUser} onClose={handleCloseEditModal} />
      )}
    </div>
  );
}
