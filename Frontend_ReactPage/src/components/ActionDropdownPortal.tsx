import { useEffect, useRef, useState } from "react";
import { createPortal } from "react-dom";

interface Props {
  anchorRef: React.RefObject<HTMLElement | null>;
  children: React.ReactNode;
  onClose: () => void;
}

export default function ActionDropdownPortal({ anchorRef, children, onClose }: Props) {
  const [position, setPosition] = useState<{ top: number; left: number }>({ top: 0, left: 0 });
  const portalRef = useRef<HTMLDivElement>(null);

  useEffect(() => {
    const anchor = anchorRef.current;
    if (anchor) {
      const rect = anchor.getBoundingClientRect();
      setPosition({ top: rect.bottom, left: rect.left });
    }

    const handleClickOutside = (e: MouseEvent) => {
      if (portalRef.current && !portalRef.current.contains(e.target as Node)) {
        onClose();
      }
    };

    document.addEventListener("mousedown", handleClickOutside);
    return () => document.removeEventListener("mousedown", handleClickOutside);
  }, [anchorRef, onClose]);

  return createPortal(
    <div
      ref={portalRef}
      style={{
        position: "absolute",
        top: position.top + window.scrollY,
        left: position.left,
        zIndex: 9999
      }}
      className="bg-white shadow rounded border"
    >
      {children}
    </div>,
    document.body
  );
}
