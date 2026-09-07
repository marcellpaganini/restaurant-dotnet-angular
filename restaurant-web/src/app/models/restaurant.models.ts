export interface Client {
  id: number;
  name: string;
  email: string;
  phone?: string | null;
  createdAt: string;
}

export interface Meal {
  id: number;
  name: string;
  description?: string | null;
  price: number;
  isAvailable: boolean;
}

export interface OrderItem {
  id: number;
  mealId: number;
  mealName?: string | null;
  quantity: number;
  unitPrice: number;
}

export interface Order {
  id: number;
  clientId: number;
  clientName?: string | null;
  orderDate: string;
  status: string;
  totalAmount: number;
  items: OrderItem[];
}
