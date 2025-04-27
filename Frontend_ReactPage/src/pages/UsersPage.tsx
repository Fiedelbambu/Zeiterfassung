// src/pages/UsersPage.tsx
import { useState } from 'react';
import UserTable from '../components/UserTable';
import CreateUserModal from '../components/CreateUserModal';

export default function UsersPage() {
  const [showCreateModal, setShowCreateModal] = useState(false);

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

      <UserTable />

      {showCreateModal && (
        <CreateUserModal onClose={() => setShowCreateModal(false)} />
      )}
    </div>
  );
}
